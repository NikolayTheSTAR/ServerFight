using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleConfig", menuName = "Data/BattleConfig")]
public class BattleConfig : ScriptableObject
{
    [SerializeField] private UnitConfigData playerData;
    [SerializeField] private UnitConfigData enemyData;

    public UnitConfigData PlayerData => playerData;
    public UnitConfigData EnemyData => enemyData;
}

[Serializable]
public struct UnitConfigData
{
    [SerializeField] private int maxHp;
    
    public int MaxHp => maxHp;
}