using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CrystalKngihtAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string closeAttackParameterName = "CloseAttack";
    [SerializeField] private string longProjectileFireParameterName = "LongProjectileFire";
    [SerializeField] private string laserFireParameterName = "LaserFire";
    [SerializeField] private string rushAttackParameterName = "RushAttack";
    
    public int IdleParameterHash { get; private set; }
    public int CloseAttackParameterHash { get; private set; }
    public int LongProjectileFireParameterHash { get; private set; }
    public int LaserFireParameterHash { get; private set; }
    public int RushAttackParameterHash { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        CloseAttackParameterHash = Animator.StringToHash(closeAttackParameterName);
        LongProjectileFireParameterHash = Animator.StringToHash(longProjectileFireParameterName);
        LaserFireParameterHash = Animator.StringToHash(laserFireParameterName);
        RushAttackParameterHash = Animator.StringToHash(rushAttackParameterName);
    }
}
