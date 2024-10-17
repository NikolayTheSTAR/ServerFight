using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager
{
    private NetworkManager network;

    [Inject]
    private void Construct(NetworkManager network)
    {
        this.network = network;
    }
}