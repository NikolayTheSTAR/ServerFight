public interface IGameClient : IGameService
{
    void VisualizeGameState(BattleState state);
    // todo должны быть методы на улавливание пользовательского взаимодействия
}