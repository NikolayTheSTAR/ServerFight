using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using DG.Tweening;
using System;

/// <summary>
/// Battle получиет текущий BattleState и преобразует его с учётом того действия, которое нужно выполнить
/// </summary>
public class Battle
{
    private Tweener waitTweener;
    private readonly ResourceHelper<BattleConfig> battleConfig = new("Configs/BattleConfig");

    public event Action<BattleState> StartEndBattleEvent;
    public event Action CompleteEndBattleEvent;
    public event Action<BattleState> OnTurnCompletedEvent;

    public void DoAbility(BattleState battleState, bool playerOwner, AbilityType abilityType)
    {
        bool canUseAbilityByGameStatus = 
            (playerOwner && battleState.battleStatus == BattleStatus.PlayerTurn) || 
            (!playerOwner && battleState.battleStatus == BattleStatus.EnemysTurn);

        if (!canUseAbilityByGameStatus) return;

        var abilityData = battleConfig.Get.Abilities[(int)abilityType];
        bool finalEffectForPlayer = abilityData.EffectForOwner ? playerOwner : !playerOwner;
        UnitState unit = finalEffectForPlayer ? battleState.playerState : battleState.enemyState;

        switch (abilityType)
        {
            case AbilityType.Attack:
                HitUnit(unit, abilityData.Force);
                break;
                
            case AbilityType.Defence:
                SetEffect(unit, EffectType.Defence, abilityData.Duration, abilityType, playerOwner);
                break;

            case AbilityType.Regenerate:
                SetEffect(unit, EffectType.Regenerate, abilityData.Duration, abilityType, playerOwner);
                break;

            case AbilityType.Fireball:
                HitUnit(unit, abilityData.Force);
                SetEffect(unit, EffectType.Fire, abilityData.Duration, abilityType, playerOwner);
                break;

            case AbilityType.Clear:
                var ownerEffects = unit.effects;
                if (ownerEffects.ContainsKey(EffectType.Fire)) RemoveEffect(battleState, unit, EffectType.Fire);
                break;
        }

        // recharging
        if (abilityData.Recharging > 0)
        {
            var ownerRecharging = playerOwner ? battleState.playerState.abilitiesRecharging : battleState.enemyState.abilitiesRecharging;
            if (ownerRecharging.ContainsKey(abilityType)) ownerRecharging[abilityType] = -1;
            else ownerRecharging.Add(abilityType, -1);
        }
        
        battleState.battleStatus = playerOwner ? BattleStatus.EnemysTurn : BattleStatus.PlayerTurn;

        if (playerOwner) SimulateWaitForEnemysTurn(battleState);
        else
        {
            OnSwitchToNextStep(battleState, battleState.playerState);
            OnSwitchToNextStep(battleState, battleState.enemyState);
        }

        OnTurnCompletedEvent?.Invoke(battleState);

        if (battleState.playerState.hp <= 0) EndFight(battleState, false);
        else if (battleState.enemyState.hp <= 0) EndFight(battleState, true);   
    }

    public void SimulateWaitForEnemysTurn(BattleState battleState)
    {
        DOVirtual.Float(0, 1, battleConfig.Get.BattleDelay, (value) => {}).OnComplete(() =>
        {
            DoEnemysTurn(battleState);
        });
    }

    private void DoEnemysTurn(BattleState battleState)
    {
        var unit = battleState.enemyState;

        List<AbilityType> availableAbilities = new();
        var allAbilities = EnumUtility.GetValues<AbilityType>();
        foreach (var ability in allAbilities)
        {
            if (!unit.abilitiesRecharging.ContainsKey(ability) || unit.abilitiesRecharging[ability] == 0) availableAbilities.Add(ability);
        }

        var decision = ArrayUtility.GetRandomValue(availableAbilities);
        DoAbility(battleState, false, decision);
    }

    private void SetEffect(UnitState unit, EffectType effectType, int duration, AbilityType fromAbility, bool fromPlayer)
    {
        var ownerEffects = unit.effects;
        if (ownerEffects.ContainsKey(effectType)) ownerEffects[effectType].value += duration;
        else ownerEffects.Add(effectType, new EffectInGameData(duration, fromAbility, fromPlayer));
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
    private void OnSwitchToNextStep(BattleState battleState, UnitState unit)
    {
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
            if (ownerEffects[effectType].value > 0)
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

                ownerEffects[effectType].value--;

                if (ownerEffects[effectType].value <= 0) RemoveEffect(battleState, unit, effectType);
            }
            else RemoveEffect(battleState, unit, effectType);
        }
    }

    void RemoveEffect(BattleState battleState, UnitState unit, EffectType effectType)
    {
        var effectData = unit.effects[effectType];
        unit.effects.Remove(effectType);
        var fromUnit = effectData.fromPlayer ? battleState.playerState : battleState.enemyState;
        
        // on remove effect
        var abilityData = battleConfig.Get.Abilities[(int)effectData.fromAbility];
        fromUnit.abilitiesRecharging[effectData.fromAbility] = abilityData.Duration;
    }

    private void EndFight(BattleState battleState, bool win)
    {
        Debug.Log(win ? "Win" : "Defeat");
        waitTweener?.Kill();
        battleState.battleStatus = win ? BattleStatus.Win : BattleStatus.Defeat;
        waitTweener = DOVirtual.Float(0f, 1f, battleConfig.Get.BattleDelay, (value) => {}).OnComplete(() => 
        {
            CompleteEndBattleEvent?.Invoke();
        });

        StartEndBattleEvent?.Invoke(battleState);
    }

    public BattleState GetInitialGameState()
    {        
        UnitState playerState = new(
            battleConfig.Get.PlayerData.MaxHp, 
            battleConfig.Get.PlayerData.MaxHp, 
            new Dictionary<EffectType, EffectInGameData>(), 
            new Dictionary<AbilityType, int>());

        UnitState enemyState = new(
            battleConfig.Get.EnemyData.MaxHp, 
            battleConfig.Get.EnemyData.MaxHp, 
            new Dictionary<EffectType, EffectInGameData>(), 
            new Dictionary<AbilityType, int>());

        return new(BattleStatus.PlayerTurn, playerState, enemyState);
    }
}