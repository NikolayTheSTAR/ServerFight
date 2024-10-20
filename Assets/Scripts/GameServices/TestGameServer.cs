using System.Threading.Tasks;
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

    private const int TestServerDelay = 10;

    [Inject]
    private void Construct(DataController data, Battle battle)
    {
        this.data = data;
        this.battle = battle;
        battle.CompleteEndBattleEvent += RestartGame;
        battle.OnTurnCompletedEvent += (state) =>
        {
            data.Save<BattleData>();
        };
    }

    public void InitGame()
    {}

    public async Task LoadGame()
    {
        await Task.Delay(TestServerDelay);
        data.LoadAll();
    }
    
    public void StartGame()
    {}

    /// <summary>
    /// В тестовой реализации данные хранятся на девайсе, для реального проекта можно было бы создать RealGameServer, который бы уже работал с сетью
    /// </summary>
    public async Task<BattleState> GetCurrentGameState()
    {
        await Task.Delay(TestServerDelay);

        var battleData = data.gameData.GetSection<BattleData>();
        
        if (battleData.gameStarted) return battleData.battleState;
        else
        {
            RestartGame();
            return battleData.battleState;
        }
    }

    public async Task<BattleState> HandleGameAction(bool playerOwner, string actionID)
    {
        // Имитация задержки отклика от сервера
        await Task.Delay(TestServerDelay);

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
            
            case "enemy_turn":
                battle.DoEnemysTurn(battleState);
                break;
        }

        return battleState;
    }

    private void RestartGame()
    {
        var battleData = data.gameData.GetSection<BattleData>();
        battle.SetInitialBattleState(battleData);
        battleData.gameStarted = true;
        data.Save(battleData);
    }
}