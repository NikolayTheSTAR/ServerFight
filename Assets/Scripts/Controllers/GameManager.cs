using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private NetworkManager network;

    [Inject]
    private void Construct(NetworkManager network)
    {
        this.network = network;
    }

    private void Start()
    {
        InitGame();
        LoadGameState();
        StartGame();
    }

    private void InitGame()
    {
        network.Init();
    }

    /// <summary>
    /// Клиент должен запросить текущее состояние игры у сервера, сервер должен вернуть либо стартовое состояние, либо сохранённое
    /// </summary>
    private void LoadGameState()
    {
        Debug.Log("INIT");
        network.LoadGameState();
    }

    /// <summary>
    /// Непосредственно начало игры, с этого момента работает обработка ввода игрока
    /// </summary>
    private void StartGame()
    {
        Debug.Log("START GAME");
    }
}