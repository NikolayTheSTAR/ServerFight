using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private GameAbVersionType versionType;

    public GameAbVersionType VersionType => versionType;
}

public enum GameAbVersionType
{
    VersionA,
    VersionB
}