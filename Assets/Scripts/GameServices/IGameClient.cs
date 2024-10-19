public interface IGameClient : IGameService
{
    void VisualizeGameState(BattleState state);
    void SendPlayerActionToServer(string actionID);
}