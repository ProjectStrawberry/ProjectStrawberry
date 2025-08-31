using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private SkeletonStateMachine stateMachine;
    
    public Animator Animator { get; private set; }
    [field:SerializeField] public SkeletonAnimationData AnimationData { get; private set; }

    private void Awake()
    {
        stateMachine = new SkeletonStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.Update();
        stateMachine.HandleInput();
    }
}
