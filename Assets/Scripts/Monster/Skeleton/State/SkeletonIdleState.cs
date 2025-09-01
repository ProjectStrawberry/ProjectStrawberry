using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonBaseState
{
    private float idleTimer = 0f;
    private float changeToWalkStateTime = 2f;
    
    public SkeletonIdleState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
        idleTimer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
        idleTimer = 0f;
    }

    public override void Update()
    {
        base.Update();
        
        idleTimer += Time.deltaTime;
        if (idleTimer >= changeToWalkStateTime)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}
