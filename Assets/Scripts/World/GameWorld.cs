using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private UnitVisual player;
    [SerializeField] private UnitVisual enemy;

    private GuiController gui;

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    [Inject]
    private void Construct(GuiController gui)
    {
        this.gui = gui;
    }

    private void Start()
    {
        var loadScreen = gui.FindScreen<LoadScreen>();

        loadScreen.Init(() =>
        {
            gui.ShowMainScreen();
        });

        gui.Show(loadScreen);
    }

    public void VisualizeGameState(GameState state)
    {
        player.VisualizeUnitState(state.PlayerState);
        enemy.VisualizeUnitState(state.EnemyState);
    }

    [ContextMenu("TestVisualize5")]
    private void TestVisualize5()
    {
        VisualizeGameState(
            new GameState(
                new UnitState(7, 10), 
                new UnitState(5, 10)));
    }

    [ContextMenu("TestVisualize1")]
    private void TestVisualize1()
    {
        VisualizeGameState(
            new GameState(
                new UnitState(1, 10), 
                new UnitState(2, 10)));
    }
}