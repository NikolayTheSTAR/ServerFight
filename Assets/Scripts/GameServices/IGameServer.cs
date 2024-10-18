using System;

public interface IGameServer : IGameService
{
    //void GetCurrentGameState(Action<BattleState> callback);
    event Action<BattleState> OnChangeGameState;
}