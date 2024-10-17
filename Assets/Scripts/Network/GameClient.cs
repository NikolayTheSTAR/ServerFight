using TheSTAR.GUI;
using UnityEngine;
using Zenject;

/// <summary>
/// Отвечает за обработку ввода от игрока, отображение интерфейса и за получение обновлений от сервера.
/// </summary>
public class GameClient : IGameService
{
    private GameWorld world;
    private GuiController gui;

    [Inject]
    private void Construct(GameWorld world, GuiController gui)
    {
        this.world = world;
        this.gui = gui;

        Debug.Log("Client connected");
    }

    public GameClient()
    {
        Debug.Log("Client created");
    }
}