using TheSTAR.GUI;
using UnityEngine;
using Zenject;

/// <summary>
/// Отвечает за обработку ввода от игрока, отображение интерфейса и за получение обновлений от сервера.
/// </summary>
public class MobileGameClient : IGameClient
{
    private GameWorld world;
    private GuiController gui;

    [Inject]
    private void Construct(GameWorld world, GuiController gui)
    {
        this.world = world;
        this.gui = gui;
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
}