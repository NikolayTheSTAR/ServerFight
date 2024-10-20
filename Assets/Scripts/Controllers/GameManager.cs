using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private IGameServer server;
    private IGameClient client;

    [Inject]
    private void Construct(IGameServer server, IGameClient client)
    {
        this.server = server;
        this.client = client;
    }

    private void Start()
    {
        PrepareGame();
    }

    private async void PrepareGame()
    {
        InitGame();
        await LoadGame();
        StartGame();
    }

    private void InitGame()
    {
        server.InitGame();
        client.InitGame();
    }

    private async Task LoadGame()
    {
        await server.LoadGame(); 
        await client.LoadGame();
    }

    private void StartGame()
    {
        server.StartGame();
        client.StartGame();
    }
}