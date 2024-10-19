using System;
using TheSTAR.Data;
using UnityEngine;
using Zenject;
using TheSTAR.Utility;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Содержит логику игры. Принимает запросы от клиента и обрабатывать их. 
/// Хранит состояние двух юнитов и обрабатывает их действия в каждом ходе.
/// Тестовым называется поскольку реализует IGameServer в тестовом виде, без реального обращения к серверу, данные хранятся на девайсе
/// </summary>
public class TestGameServer : IGameServer
{
    private DataController data;

    private readonly ResourceHelper<BattleConfig> battleConfig = new("Configs/BattleConfig");

    public event Action<BattleState> OnChangeGameState;

    private Tweener waitTweener;

    [Inject]
    private void Construct(DataController data)
    {
        this.data = data;
    }

    public void InitGame()
    {}

    public void LoadGame()
    {
        data.LoadAll();
        GetCurrentGameState(state =>
        {
            OnChangeGameState?.Invoke(state);
        });
    }

    public void StartGame()
    {
        var battleData = data.gameData.GetSection<BattleData>();
        if (battleData.battleState.battleStatus == BattleStatus.EnemysTurn) SimulateWaitForEnemysTurn();
    }

    /// <summary>
    /// В тестовой реализации данные хранятся на девайсе, для реального проекта можно было бы создать RealGameServer, который бы уже работал с сетью
    /// </summary>
    private void GetCurrentGameState(Action<BattleState> callback)
    {
        var battleData = data.gameData.GetSection<BattleData>();
        
        if (battleData.gameStarted) callback(battleData.battleState);
        else
        {
            RestartGame();
            callback(battleData.battleState);
        }
    }

    private BattleState GetInitialGameState()
    {        
        UnitState playerState = new(
            battleConfig.Get.PlayerData.MaxHp, 
            battleConfig.Get.PlayerData.MaxHp, 
            new Dictionary<EffectType, int>(), 
            new Dictionary<AbilityType, int>());

        UnitState enemyState = new(
            battleConfig.Get.EnemyData.MaxHp, 
            battleConfig.Get.EnemyData.MaxHp, 
            new Dictionary<EffectType, int>(), 
            new Dictionary<AbilityType, int>());

        return new(BattleStatus.PlayerTurn, playerState, enemyState);
    }

    public void HandleGameAction(bool playerOwner, string actionID)
    {
        switch (actionID)
        {
            case "restart":
                RestartGame();
                break;

            case "ability_Attack":
                DoAbility(playerOwner, AbilityType.Attack);
                break;

            case "ability_Defence":
                DoAbility(playerOwner, AbilityType.Defence);
                break;

            case "ability_Regenerate":
                DoAbility(playerOwner, AbilityType.Regenerate);
                break;

            case "ability_Fireball":
                DoAbility(playerOwner, AbilityType.Fireball);
                break;

            case "ability_Clear":
                DoAbility(playerOwner, AbilityType.Clear);
                break;
        }
    }

    private void RestartGame()
    {
        waitTweener?.Kill();
        var battleData = data.gameData.GetSection<BattleData>();

        var startState = GetInitialGameState();
        battleData.battleState = startState;
        battleData.gameStarted = true;
        data.Save(battleData);

        OnChangeGameState?.Invoke(startState);
    }

    private void DoAbility(bool playerOwner, AbilityType abilityType)
    {
        var battleData = data.gameData.GetSection<BattleData>();

        bool canUseAbilityByGameStatus = 
            (playerOwner && battleData.battleState.battleStatus == BattleStatus.PlayerTurn) || 
            (!playerOwner && battleData.battleState.battleStatus == BattleStatus.EnemysTurn);

        if (!canUseAbilityByGameStatus) return;

        var abilityData = battleConfig.Get.Abilities[(int)abilityType];
        bool finalEffectForPlayer = abilityData.EffectForOwner ? playerOwner : !playerOwner;
        UnitState unit = finalEffectForPlayer ? battleData.battleState.playerState : battleData.battleState.enemyState;

        switch (abilityType)
        {
            case AbilityType.Attack:
                HitUnit(unit, abilityData.Force);
                break;
                
            case AbilityType.Defence:
                SetEffect(unit, EffectType.Defence, abilityData.Duration);
                break;

            case AbilityType.Regenerate:
                SetEffect(unit, EffectType.Regenerate, abilityData.Duration);
                break;

            case AbilityType.Fireball:
                HitUnit(unit, abilityData.Force);
                SetEffect(unit, EffectType.Fire, abilityData.Duration);
                break;

            case AbilityType.Clear:
                var ownerEffects = playerOwner ? battleData.battleState.playerState.effects : battleData.battleState.enemyState.effects;
                if (ownerEffects.ContainsKey(EffectType.Fire)) ownerEffects.Remove(EffectType.Fire);
                break;
        }

        // recharging
        if (abilityData.Recharging > 0)
        {
            var ownerRecharging = playerOwner ? battleData.battleState.playerState.abilitiesRecharging : battleData.battleState.enemyState.abilitiesRecharging;
            if (ownerRecharging.ContainsKey(abilityType)) ownerRecharging[abilityType] = abilityData.Recharging;
            else ownerRecharging.Add(abilityType, abilityData.Recharging);
        }
        
        battleData.battleState.battleStatus = playerOwner ? BattleStatus.EnemysTurn : BattleStatus.PlayerTurn;
        data.Save(battleData);

        OnChangeGameState?.Invoke(battleData.battleState);

        // проверка умер ли персонаж либо враг происходит после каждого хода

        if (battleData.battleState.playerState.hp <= 0) Defeat();
        else if (battleData.battleState.enemyState.hp <= 0) Win();
        else if (playerOwner) SimulateWaitForEnemysTurn();

        // todo это потом расположить в нужном месте
        //OnSwitchToNextStep(battleData.battleState.playerState);
        //OnSwitchToNextStep(battleData.battleState.enemyState);
    }

    private void SimulateWaitForEnemysTurn()
    {
        DOVirtual.Float(0, 1, battleConfig.Get.EnemyDecisionTime, (value) => {}).OnComplete(() =>
        {
            DoEnemysTurn();
        });
    }

    private void DoEnemysTurn()
    {
        var battleData = data.gameData.GetSection<BattleData>();
        var unit = battleData.battleState.enemyState;

        List<AbilityType> availableAbilities = new();
        var allAbilities = EnumUtility.GetValues<AbilityType>();
        foreach (var ability in allAbilities)
        {
            if (!unit.abilitiesRecharging.ContainsKey(ability) || unit.abilitiesRecharging[ability] <= 0) availableAbilities.Add(ability);
        }

        var decision = ArrayUtility.GetRandomValue(availableAbilities);
        DoAbility(false, decision);
    }

    private void SetEffect(UnitState unit, EffectType effectType, int duration)
    {
        var ownerEffects = unit.effects;
        if (ownerEffects.ContainsKey(effectType)) ownerEffects[effectType] += duration;
        else ownerEffects.Add(effectType, duration);
    }

    private void HitUnit(UnitState unit, int force, bool ignoreDefence = false)
    {
        if (!ignoreDefence && unit.effects.ContainsKey(EffectType.Defence)) force -= battleConfig.Get.Abilities[(int)AbilityType.Defence].Force;

        if (force <= 0) return;
        
        unit.hp -= force;
        if (unit.hp < 0) unit.hp = 0;
    }

    private void HillUnit(UnitState unit, int force)
    {
        unit.hp += force;
        if (unit.hp > unit.maxHp) unit.hp = unit.maxHp;
    }

    /// <summary>
    /// Перешли на новый ход, нужно сделать шаг перезарядки и эффекта
    /// </summary>
    private void OnSwitchToNextStep(UnitState unit)
    {
        var battleData = data.gameData.GetSection<BattleData>();

        // recharging
        var ownerRecharging = unit.abilitiesRecharging;
        
        AbilityType[] abilityTypes = new AbilityType[ownerRecharging.Count];
        int abilityIndex = 0;
        foreach (var recharging in ownerRecharging)
        {
            abilityTypes[abilityIndex] = recharging.Key;
            abilityIndex++;
        }
        
        foreach (var abilityType in abilityTypes)
        {
            if (ownerRecharging[abilityType] > 0) ownerRecharging[abilityType]--;
        }

        // effects
        var ownerEffects = unit.effects;

        EffectType[] effectTypes = new EffectType[ownerEffects.Count];
        int effectIndex = 0;
        foreach (var effect in ownerEffects)
        {
            effectTypes[effectIndex] = effect.Key;
            effectIndex++;
        }

        foreach (var effectType in effectTypes)
        {
            if (ownerEffects[effectType] > 0)
            {
                if (effectType == EffectType.Regenerate)
                {
                    int regenerationForce = battleConfig.Get.Abilities[(int)AbilityType.Regenerate].Force;
                    HillUnit(unit, regenerationForce);
                }
                else if (effectType == EffectType.Fire)
                {
                    int fireForce = 1;
                    HitUnit(unit, fireForce, true);
                }

                ownerEffects[effectType]--;

                if (ownerEffects[effectType] <= 0) ownerEffects.Remove(effectType);
            }
            else ownerEffects.Remove(effectType);
        }

        data.Save(battleData);

        OnChangeGameState?.Invoke(battleData.battleState);
    }

    private void Win()
    {
        waitTweener?.Kill();
        var battleData = data.gameData.GetSection<BattleData>();
        battleData.battleState.battleStatus = BattleStatus.Win;
        data.Save(battleData);
        waitTweener = DOVirtual.Float(0f, 1f, 1f, (value) => {}).OnComplete(RestartGame);
        OnChangeGameState?.Invoke(battleData.battleState);
    }

    private void Defeat()
    {
        waitTweener?.Kill();
        var battleData = data.gameData.GetSection<BattleData>();
        battleData.battleState.battleStatus = BattleStatus.Defeat;
        data.Save(battleData);
        waitTweener = DOVirtual.Float(0f, 1f, 1f, (value) => {}).OnComplete(RestartGame);
        OnChangeGameState?.Invoke(battleData.battleState);
    }
}