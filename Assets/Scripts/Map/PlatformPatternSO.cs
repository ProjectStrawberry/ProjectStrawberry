using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlatformType
{
    
    SemiSolid,
    Solid
}

public enum PlatformPlacement
{
    Single,
    Double
}

[CreateAssetMenu(fileName ="PlatformData",menuName ="Platform/PlatformPattern")]
public class PlatformPatternSO : ScriptableObject
{
    public int patternNumber;
    public PlatformData[] platformDatas;
}

[Serializable]
public class PlatformData
{
    public Vector3 platformPosition;
    public PlatformType platformType;
    public PlatformPlacement platformPlacement;
    public float platformWidth;

}