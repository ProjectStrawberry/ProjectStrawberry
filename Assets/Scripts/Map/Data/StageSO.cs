using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterType
{
    Skeleton,
    FloatingSkull,
    CrystalKnight
}

[Serializable]
public class MonsterSpawnData
{
    public MonsterType type;
    public Vector2[] spawnPoints;
}

[Serializable]
public class StageLevel
{
    public int currentStageLevel;
    public MonsterSpawnData[] monsters;
}

[CreateAssetMenu(fileName = "New StageSO", menuName = "Stage/New Stage")]
public class StageSO : ScriptableObject
{
    public StageLevel[] stages;
}
