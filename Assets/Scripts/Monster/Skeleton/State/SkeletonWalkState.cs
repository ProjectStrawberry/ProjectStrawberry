using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWalkState : SkeletonBaseState
{
    public SkeletonWalkState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
    }
    
    public override void Update()
    {
        base.Update();
        // 애니메이션 동작 확인용
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
