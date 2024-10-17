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

        Debug.Log("Client connected");
    }

    public MobileGameClient()
    {
        Debug.Log("Client created");
    }
}