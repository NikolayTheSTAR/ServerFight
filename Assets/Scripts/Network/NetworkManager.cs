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

    public void InitGame()
    {
    }
}

public struct GameState
{
    private UnitState playerState;
    private UnitState enemyState;

    public UnitState PlayerState => playerState;
    public UnitState EnemyState => enemyState;

    public GameState(UnitState playerState, UnitState enemyState)
    {
        this.playerState = playerState;
        this.enemyState = enemyState;
    }
}

public struct UnitState
{
    private int hp;
    private int maxHp;

    public int HP => hp;
    public int MaxHP => maxHp;

    public UnitState(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
    }

    // todo тут так же должна быть информация по перезарядке способностей и по ходам
}