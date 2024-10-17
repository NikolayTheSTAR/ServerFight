using UnityEngine;

/// <summary>
/// Отвечает за обработку ввода от игрока, отображение интерфейса и за получение обновлений от сервера.
/// </summary>
public class GameClient : IGameService
{
    public GameClient()
    {
        Debug.Log("Client created");
    }
}