using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonBaseState
{
    public SkeletonIdleState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        // 애니메이션 동작 확인용
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }
}
