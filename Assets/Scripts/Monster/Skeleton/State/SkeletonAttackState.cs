using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : SkeletonBaseState
{
    private bool isAttacking = false;
    
    public SkeletonAttackState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Attack 상태 입장");
        isAttacking = false;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Attack 상태 퇴장");
        StopAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }
    
    public override void Update()
    {
        base.Update();

        if (isAttacking == false)
        {
            if (IsInDefaultAttackRange())
            {
                Debug.Log("근접 공격 실행");
                isAttacking = true;
                DefaultAttack();
            }
            else if (IsInRushAttackRange())
            {
                Debug.Log("돌진 공격 실행");
                isAttacking = true;
                RushAttack();
            }
        }
    }

    private void DefaultAttack()
    {
        Debug.Log("근접 공격 실행2");
        StartAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }

    private void RushAttack()
    {
        Debug.Log("돌진 공격 실행2");
        StartAnimation(stateMachine.Skeleton.AnimationData.RushAttackParameterHash);
    }

    public void StopAttackAnimation()
    {
        StopAnimation(stateMachine.Skeleton.AnimationData.AttackParameterHash);
    }
    
    public void StopRushAttackAnimation()
    {
        StopAnimation(stateMachine.Skeleton.AnimationData.RushAttackParameterHash);
    }
}
