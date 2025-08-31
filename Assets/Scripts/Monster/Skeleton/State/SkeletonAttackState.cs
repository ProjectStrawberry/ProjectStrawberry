using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : SkeletonBaseState
{
    public SkeletonAttackState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
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
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
