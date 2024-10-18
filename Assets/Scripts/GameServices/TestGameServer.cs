using System;
using TheSTAR.Data;
using UnityEngine;
using Zenject;
using TheSTAR.Utility;

/// <summary>
/// Содержит логику игры. Принимает запросы от клиента и обрабатывать их. 
/// Хранит состояние двух юнитов и обрабатывает их действия в каждом ходе.
/// Тестовым называется поскольку реализует IGameServer в тестовом виде, без реального обращения к серверу, данные хранятся на девайсе
/// </summary>
public class TestGameServer : IGameServer
{
    private DataController data;

    private readonly ResourceHelper<BattleConfig> battleConfig = new("Configs/BattleConfig");

    public event Action<BattleState> OnChangeGameState;

    [Inject]
    private void Construct(DataController data)
    {
        this.data = data;
    }

    public void InitGame()
    {}

    public void LoadGame()
    {
        data.LoadAll();
        GetCurrentGameState(state =>
        {
            OnChangeGameState?.Invoke(state);
        });
    }

    public void StartGame()
    {}

    /// <summary>
    /// В тестовой реализации данные хранятся на девайсе, для реального проекта можно было бы создать RealGameServer, который бы уже работал с сетью
    /// </summary>
    private void GetCurrentGameState(Action<BattleState> callback)
    {
        var battleData = data.gameData.GetSection<BattleData>();
        
        // игра уже в процессе, возвращаем текущее состояние
        if (battleData.gameStarted)
        {
            Debug.Log("Загрузка сохранённого состояния игры");
            callback(battleData.battleState);
        }

        // игра ещё не начиналась, возвращаем исходное состояние
        else
        {
            Debug.Log("Новая игра");
            var startState = GetStartGameState();
            battleData.battleState = startState;
            battleData.gameStarted = true;
            data.Save(battleData);
            callback(startState);
        }
    }

    private BattleState GetStartGameState()
    {        
        UnitState playerState = new(battleConfig.Get.PlayerData.MaxHp, battleConfig.Get.PlayerData.MaxHp);
        UnitState enemyState = new(battleConfig.Get.EnemyData.MaxHp, battleConfig.Get.EnemyData.MaxHp);
        return new(playerState, enemyState);
    }
}