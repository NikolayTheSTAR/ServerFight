using Zenject;

/// <summary>
/// Отвечает за сетевое взаимодействие
/// </summary>
public class NetworkManager
{
    // todo должен композировать клиент и сервет, должен выстроить взаимосвязсь между ними

    private GameClient client;
    private GameServer server;

    [Inject]
    private void Construct(GameClient client, GameServer server)
    {
        this.client = client;
        this.server = server;
    }
}