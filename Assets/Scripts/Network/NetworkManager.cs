using System;
using Newtonsoft.Json;
using Zenject;

/// <summary>
/// Отвечает за сетевое взаимодействие
/// </summary>
public class NetworkManager
{
    private IGameClient client;
    private IGameServer server;

    [Inject]
    private void Construct(IGameClient client, IGameServer server)
    {
        this.client = client;
        this.server = server;
    }

    public void Init()
    {
        server.Init();
    }

    public void LoadGameState()
    {
        server.GetCurrentGameState(state => client.VisualizeGameState(state));
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