using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private IGameClient client;
    private IGameServer server;

    [Inject]
    private void Construct(IGameClient client, IGameServer server)
    {
        this.client = client;
        this.server = server;
    }

    private void Start()
    {
        // todo тут учесть что может быть задержка между LoadGame и StartGame
        InitGame();
        LoadGame();
        StartGame();
    }

    private void InitGame()
    {
        client.InitGame();
        server.InitGame();
    }

    private void LoadGame()
    {
        client.LoadGame();
        server.LoadGame(); 
    }

    private void StartGame()
    {
        client.StartGame();
        server.StartGame();
    }
}

[Serializable]
public struct BattleState
{
    public BattleStatus battleStatus;
    public UnitState playerState;
    public UnitState enemyState;

    public BattleState(BattleStatus battleStatus, UnitState playerState, UnitState enemyState)
    {
        this.battleStatus = battleStatus;
        this.playerState = playerState;
        this.enemyState = enemyState;
    }
}

[Serializable]
public class UnitState
{
    public int hp;
    public int maxHp;

    // 0 означает что эффект неактивен, 
    // 1 и больше означает количество ходов сколько ещё эффект будет активен
    public Dictionary<EffectType, EffectInGameData> effects;

    // 0 означает что способность можно использовать, 
    // -1 означает что способность использована и ожидает начала перезарядки,
    // 1 и больше означает сколько ходов ещё будет идти перезарядка
    public Dictionary<AbilityType, int> abilitiesRecharging;

    public UnitState(
        int hp, 
        int maxHp, 
        Dictionary<EffectType, EffectInGameData> effects, 
        Dictionary<AbilityType, int> abilitiesRecharging)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.effects = effects;
        this.abilitiesRecharging = abilitiesRecharging;
    }    
}

public enum BattleStatus
{
    PlayerTurn,
    EnemysTurn,
    Win,
    Defeat
}

public class EffectInGameData
{
    public int value;
    public AbilityType fromAbility;
    public bool fromPlayer;

    public EffectInGameData(int value, AbilityType fromAbility, bool fromPlayer)
    {
        this.value = value;
        this.fromAbility = fromAbility;
        this.fromPlayer = fromPlayer;
    }
}