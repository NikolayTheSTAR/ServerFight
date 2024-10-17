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
}

public struct UnitState
{
    private int hp;
    public int HP => hp;

    // todo тут так же должна быть информация по перезарядке способностей и по ходам
}

public interface IGameClient
{

}

public interface IGameServer
{

}