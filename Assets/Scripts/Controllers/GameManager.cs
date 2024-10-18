using System;
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
}

[Serializable]
public struct BattleState
{
    public UnitState playerState;
    public UnitState enemyState;

    public BattleState(UnitState playerState, UnitState enemyState)
    {
        this.playerState = playerState;
        this.enemyState = enemyState;
    }
}

[Serializable]
public struct UnitState
{
    public int hp;
    public int maxHp;

    public UnitState(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
    }

    // todo тут так же должна быть информация по перезарядке способностей и по ходам
}