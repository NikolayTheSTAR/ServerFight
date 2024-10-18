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
                new UnitState(7, 10, new UnitEffectState[]{}), 
                new UnitState(5, 10, new UnitEffectState[]{})));
    }

    [ContextMenu("TestVisualize1")]
    private void TestVisualize1()
    {
        VisualizeGameState(
            new BattleState(
                new UnitState(1, 10, new UnitEffectState[]
                {
                    new UnitEffectState(EffectType.Defence, 3),
                    new UnitEffectState(EffectType.Regenerate, 1),
                }), 
                new UnitState(2, 10, new UnitEffectState[]
                {
                    new UnitEffectState(EffectType.Fire, 2)
                })));
    }
}