using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Stemina,
    Speed,
    JumpeForce,
    CutJumpForceMultiplier,
    FirstAttack,
    SecondAttack,
    AttackingSlow,
    RangeAttack,
    ProjectileVelocity,
    ProjectileDuration,
    AttackDelay,
    RangeAttackDelay,
    DashDuration,
    DashCoolTime,
    DashDistance,
    HealAmount, //얼마나 회복
    HealDelay,  //몇 초 뒤 회복
    HealSteminaConsumeInterval,  //몇 초 간격으로
    DamagedInvincibleDuration,
    DamagedAnimatingDuration,
    ComboResetTime,
    RequireAttackForStemina
}

[System.Serializable]
public class StatEntry
{
    public StatType statType;
    public float baseValue;
}

[CreateAssetMenu(fileName = "New StatData", menuName = "NewPlayerStats")]
public class StatData : ScriptableObject
{
    public string characterName;
    public List<StatEntry> stats;
}

