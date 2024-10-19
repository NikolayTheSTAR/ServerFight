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

    [ContextMenu("TestVisualize1")]
    private void TestVisualize1()
    {
        client.VisualizeGameState(
            new BattleState(
                BattleStatus.PlayerTurn,
                new UnitState(7, 10, new Dictionary<EffectType, int>(), new Dictionary<AbilityType, int>()
                {
                    {AbilityType.Regenerate, 1},
                }), 
                new UnitState(5, 10, new Dictionary<EffectType, int>(), new Dictionary<AbilityType, int>())));
    }

    [ContextMenu("TestVisualize2")]
    private void TestVisualize2()
    {
        client.VisualizeGameState(
            new BattleState(
                BattleStatus.EnemysTurn,
                new UnitState(1, 10, new Dictionary<EffectType, int>()
                {
                    {EffectType.Defence, 3},
                    {EffectType.Regenerate, 2},
                }, new Dictionary<AbilityType, int>()
                {
                    {AbilityType.Fireball, 5},
                    {AbilityType.Regenerate, 2},
                }), 
                new UnitState(2, 10, new Dictionary<EffectType, int>()
                {
                    {EffectType.Fire, 2}
                }, new Dictionary<AbilityType, int>())));
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
    public Dictionary<EffectType, int> effects;
    public Dictionary<AbilityType, int> abilitiesRecharging;

    public UnitState(
        int hp, 
        int maxHp, 
        Dictionary<EffectType, int> effects, 
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