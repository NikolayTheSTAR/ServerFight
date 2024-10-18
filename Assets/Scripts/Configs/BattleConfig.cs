using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleConfig", menuName = "Data/BattleConfig")]
public class BattleConfig : ScriptableObject
{
    [Header("Units")]
    [SerializeField] private UnitConfigData playerData;
    [SerializeField] private UnitConfigData enemyData;

    [Header("Skills")]
    [SerializeField] private SkillConfigData[] skills;

    [Header("Effects")]
    [SerializeField] private EffectConfigData[] effects;

    public UnitConfigData PlayerData => playerData;
    public UnitConfigData EnemyData => enemyData;
    public SkillConfigData[] Skills => skills;
    public EffectConfigData[] Effects => effects;
}

[Serializable]
public struct UnitConfigData
{
    [SerializeField] private int maxHp;
    
    public int MaxHp => maxHp;
}

[Serializable]
public struct SkillConfigData
{
    [SerializeField] private SkillType skillType;
    [SerializeField] private SkillTargetType target;
    [SerializeField] private int force;
    [SerializeField] private int duration;
    [SerializeField] private int recharging;
}

[Serializable]
public struct EffectConfigData
{
    [SerializeField] private EffectType effectType;
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;
}

public enum SkillType
{
    /// <summary> применяется на противника. Наносит 8 единиц урона. </summary>
    Attack,

    /// <summary> применяется на себя, блокирует суммарно 5 единиц урона. Длительность 2 хода, перезарядка 4 хода. </summary>
    Defence,

    /// <summary> применяется на себя, каждый ход восстанавливает 2 единицы жизни. </summary>
    Regenerate,

    /// <summary> при использовании наносит противнику 5 урона, после чего накладывается эффект, наносящий каждый ход 1 урона противнику. Длительность 5 ходов, перезарядка 6 ходов. </summary>
    Fireball,

    /// <summary> применяется на себя, снимает эффект горения. Перезарядка 5 ходов. </summary>
    Clear
}

public enum EffectType
{
    Defence,
    Regenerate,
    Fire
}

public enum SkillTargetType
{
    Self,
    Other
}