using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private UnitVisual player;
    [SerializeField] private UnitVisual enemy;

    private GuiController gui;
    private GameManager gameManager;

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
}