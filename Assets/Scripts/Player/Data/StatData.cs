using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Speed,
    JumpeForce,
    CutJumpForceMultiplier,
    Attack,
    RangeAttack,
    DashDuration,
    DashCoolTime,
    DashDistance
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

