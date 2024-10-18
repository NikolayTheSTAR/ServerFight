using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private UnitVisual player;
    [SerializeField] private UnitVisual enemy;

    public void VisualizeGameState(BattleState state)
    {
        player.VisualizeUnitState(state.playerState);
        enemy.VisualizeUnitState(state.enemyState);
    }

    [ContextMenu("TestVisualize5")]
    private void TestVisualize5()
    {
        VisualizeGameState(
            new BattleState(
                new UnitState(7, 10), 
                new UnitState(5, 10)));
    }

    [ContextMenu("TestVisualize1")]
    private void TestVisualize1()
    {
        VisualizeGameState(
            new BattleState(
                new UnitState(1, 10), 
                new UnitState(2, 10)));
    }
}