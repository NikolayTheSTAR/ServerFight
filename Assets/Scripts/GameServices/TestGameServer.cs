using System;
using TheSTAR.Data;
using Zenject;

/// <summary>
/// Содержит логику игры. Принимает запросы от клиента и обрабатывать их. 
/// Хранит состояние двух юнитов и обрабатывает их действия в каждом ходе.
/// Тестовым называется поскольку реализует IGameServer в тестовом виде, без реального обращения к серверу, данные хранятся на девайсе
/// </summary>
public class TestGameServer : IGameServer
{
    private DataController data;
    private Battle battle;

    public event Action<BattleState> OnChangeGameState;

    [Inject]
    private void Construct(DataController data, Battle battle)
    {
        this.data = data;
        this.battle = battle;
        battle.CompleteEndBattleEvent += RestartGame;
        battle.StartEndBattleEvent += (state) =>
        {
            data.Save<BattleData>();
            OnChangeGameState(state);
        };
        battle.OnTurnCompletedEvent += (state) =>
        {
            data.Save<BattleData>();
            OnChangeGameState(state);
        };
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
    {
        var battleData = data.gameData.GetSection<BattleData>();
        if (battleData.battleState.battleStatus == BattleStatus.EnemysTurn) battle.SimulateWaitForEnemysTurn(battleData.battleState);
    }

    /// <summary>
    /// В тестовой реализации данные хранятся на девайсе, для реального проекта можно было бы создать RealGameServer, который бы уже работал с сетью
    /// </summary>
    private void GetCurrentGameState(Action<BattleState> callback)
    {
        var battleData = data.gameData.GetSection<BattleData>();
        
        if (battleData.gameStarted) callback(battleData.battleState);
        else
        {
            RestartGame();
            callback(battleData.battleState);
        }
    }

    public void HandleGameAction(bool playerOwner, string actionID)
    {
        var battleData = data.gameData.GetSection<BattleData>();
        var battleState = battleData.battleState;

        switch (actionID)
        {
            case "restart":
                RestartGame();
                break;

            case "ability_Attack":
                battle.DoAbility(battleState, playerOwner, AbilityType.Attack);
                break;

            case "ability_Defence":
                battle.DoAbility(battleState, playerOwner, AbilityType.Defence);
                break;

            case "ability_Regenerate":
                battle.DoAbility(battleState, playerOwner, AbilityType.Regenerate);
                break;

            case "ability_Fireball":
                battle.DoAbility(battleState, playerOwner, AbilityType.Fireball);
                break;

            case "ability_Clear":
                battle.DoAbility(battleState, playerOwner, AbilityType.Clear);
                break;
        }
    }

    private void RestartGame()
    {
        var battleData = data.gameData.GetSection<BattleData>();

        var startState = battle.GetInitialGameState();
        battleData.battleState = startState;
        battleData.gameStarted = true;
        data.Save(battleData);

        OnChangeGameState?.Invoke(startState);
    }
}