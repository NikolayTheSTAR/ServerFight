public interface IGameClient : IGameService
{
    void VisualizeGameState(BattleState state);
    void SendGameActionToServer(string actionID);
}