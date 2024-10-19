using TheSTAR.GUI;
using Zenject;

/// <summary>
/// Отвечает за обработку ввода от игрока, отображение интерфейса и за получение обновлений от сервера.
/// </summary>
public class MobileGameClient : IGameClient
{
    private GameWorld world;
    private GuiController gui;
    private IGameServer server;

    [Inject]
    private void Construct(GameWorld world, GuiController gui, IGameServer server)
    {
        this.world = world;
        this.gui = gui;
        this.server = server;
        server.OnChangeGameState += VisualizeGameState;
    }

    public void InitGame()
    {}

    public void LoadGame()
    {
        gui.Show<LoadScreen>();
    }

    public void StartGame()
    {
        gui.Show<GameScreen>();
    }

    public void VisualizeGameState(BattleState state)
    {
        world.VisualizeGameState(state);
        gui.FindScreen<GameScreen>().VisualizeGameState(state);
    }

    public void SendPlayerActionToServer(string actionID)
    {
        server.HandleGameAction(true, actionID);
    }
}