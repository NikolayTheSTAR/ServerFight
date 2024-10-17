using UnityEngine;

/// <summary>
/// Содержит логику игры. Принимает запросы от клиента и обрабатывать их. 
/// Хранит состояние двух юнитов и обрабатывает их действия в каждом ходе.
/// </summary>
public class GameServer : IGameService
{
    public GameServer()
    {
        Debug.Log("Server created");
    }
}