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
        Debug.Log("Attack 상태 입장");
        if (IsInDefaultAttackRange())
        {
            Debug.Log("근접 공격 실행");
            DefaultAttack();
        }
        else if (IsInRushAttackRange())
        {
            Debug.Log("돌진 공격 실행");
            DefaultAttack();
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }
    
    public override void Update()
    {
        base.Update();
    }

    private void DefaultAttack()
    {
        Debug.Log("근접 공격 실행2");
        StartAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }

    private void RushAttack()
    {
        Debug.Log("돌진 공격 실행2");
        StartAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }

    public void StopAttackAnimation()
    {
        StopAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }
}
