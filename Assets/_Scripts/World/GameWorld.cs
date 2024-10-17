using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameWorld : MonoBehaviour
{
    private GuiController gui;
    private GameManager gameManager;

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    [Inject]
    private void Construct(GuiController gui, GameManager gameManager)
    {
        this.gui = gui;
        this.gameManager = gameManager;
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