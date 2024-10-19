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
        server.OnChangeGameState += client.VisualizeGameState;
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
                true,
                new UnitState(7, 10, new UnitEffectState[]{}, new Dictionary<SkillType, int>()
                {
                    {SkillType.Regenerate, 1},
                }), 
                new UnitState(5, 10, new UnitEffectState[]{}, new Dictionary<SkillType, int>())));
    }

    [ContextMenu("TestVisualize2")]
    private void TestVisualize2()
    {
        client.VisualizeGameState(
            new BattleState(
                false,
                new UnitState(1, 10, new UnitEffectState[]
                {
                    new UnitEffectState(EffectType.Defence, 3),
                    new UnitEffectState(EffectType.Regenerate, 2),
                }, new Dictionary<SkillType, int>()
                {
                    {SkillType.Fireball, 5},
                    {SkillType.Regenerate, 2},
                }), 
                new UnitState(2, 10, new UnitEffectState[]
                {
                    new UnitEffectState(EffectType.Fire, 2)
                }, new Dictionary<SkillType, int>())));
    }
}

[Serializable]
public struct BattleState
{
    public bool playersTurn;
    public UnitState playerState;
    public UnitState enemyState;

    public BattleState(bool playersTurn, UnitState playerState, UnitState enemyState)
    {
        this.playersTurn = playersTurn;
        this.playerState = playerState;
        this.enemyState = enemyState;
    }
}

[Serializable]
public struct UnitState
{
    public int hp;
    public int maxHp;
    public UnitEffectState[] effects;
    public Dictionary<SkillType, int> skillsRecharging;

    public UnitState(int hp, int maxHp, UnitEffectState[] effects, Dictionary<SkillType, int> skillsRecharging)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.effects = effects;
        this.skillsRecharging = skillsRecharging;
    }    
}

[Serializable]
public struct UnitEffectState
{
    public EffectType effectType;
    public int value;

    public UnitEffectState(EffectType effectType, int value)
    {
        this.effectType = effectType;
        this.value = value;
    }
}