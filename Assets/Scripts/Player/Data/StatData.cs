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
    HealAmount, //�󸶳� ȸ��
    HealDelay,  //�� �� �� ȸ��
    HealSteminaConsumeInterval,  //�� �� ��������
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

