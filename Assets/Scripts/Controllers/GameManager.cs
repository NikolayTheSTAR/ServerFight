using System.Collections;
using System.Collections.Generic;
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
        StartGame();
    }

    private void InitGame()
    {
        Debug.Log("INIT");
        network.InitGame();
    }

    private void StartGame()
    {
        Debug.Log("START GAME");

        // todo нужно достать текущее состояние игры с сервера
    }
}