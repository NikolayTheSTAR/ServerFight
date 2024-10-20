using TheSTAR.GUI;
using Zenject;
using DG.Tweening;
using TheSTAR.Utility;
using System.Threading.Tasks;

/// <summary>
/// Отвечает за обработку ввода от игрока, отображение интерфейса и за получение обновлений от сервера.
/// </summary>
public class MobileGameClient : IGameClient
{
    private GameWorld world;
    private GuiController gui;
    private IGameServer server;

    private readonly ResourceHelper<BattleConfig> battleConfig = new("Configs/BattleConfig");

    [Inject]
    private void Construct(GameWorld world, GuiController gui, IGameServer server)
    {
        this.world = world;
        this.gui = gui;
        this.server = server;
    }

    public void InitGame()
    {}

    public async Task LoadGame()
    {
        gui.Show<LoadScreen>();

        var battleData = await server.GetCurrentGameState();
        VisualizeGameState(battleData);
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

    public async void SendGameActionToServer(string actionID)
    {
        var battleState = await server.HandleGameAction(true, actionID);
        VisualizeGameState(battleState);

        if (battleState.battleStatus == BattleStatus.EnemysTurn) SimulateWaitForEnemysTurn();
    }

    private void SimulateWaitForEnemysTurn()
    {
        DOVirtual.Float(0, 1, battleConfig.Get.BattleDelay, (value) => {}).OnComplete(() =>
        {
            SendGameActionToServer("enemy_turn");
        });
    }
}