using System.Threading.Tasks;

public interface IGameServer : IGameService
{
    Task<BattleState> GetCurrentGameState();
    Task<BattleState> HandleGameAction(bool playerOwner, string actionID);
}