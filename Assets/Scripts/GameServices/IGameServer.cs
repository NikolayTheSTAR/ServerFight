using System;

public interface IGameServer : IGameService
{
    event Action<BattleState> OnChangeGameState;
    void HandleGameAction(bool playerOwner, string actionID);
}