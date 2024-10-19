using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private UnitVisual player;
    [SerializeField] private UnitVisual enemy;

    public void VisualizeGameState(BattleState state)
    {
        player.VisualizeUnitState(state.playerState);
        enemy.VisualizeUnitState(state.enemyState);
    }
}