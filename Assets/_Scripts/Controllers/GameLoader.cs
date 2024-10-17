using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");
    public GameConfig GameConfig => gameConfig.Get;

    private void Start()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}

public enum GameAbVersionType
{
    VersionA,
    VersionB
}