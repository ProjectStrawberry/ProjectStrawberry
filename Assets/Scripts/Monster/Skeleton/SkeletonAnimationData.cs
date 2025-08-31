using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkeletonAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string attackParameterName = "Attack";
    
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
    }
}
