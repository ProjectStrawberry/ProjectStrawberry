using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationHandler : MonoBehaviour
{
    public Skeleton Skeleton;

    private void Awake()
    {
        Skeleton = GetComponentInParent<Skeleton>();
    }

    public void OnAttackAnimationStart()
    {
        Skeleton.AttackColliderHandler.TurnOnAttackCollider();
    }

    public void OnAttackAnimationEnd()
    {
        Skeleton.AttackColliderHandler.TurnOffAttackCollider();
        Skeleton.stateMachine.AttackState.StopAttackAnimation();
        Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.ChasingState);
    }

    public void OnRushAttackAnimationStart()
    {
        Skeleton.AttackColliderHandler.TurnOnAttackCollider();
        Skeleton.RushAttackMove();
    }

    public void OnRushAttackAnimationEnd()
    {
        Skeleton.AttackColliderHandler.TurnOffAttackCollider();
        Skeleton.stateMachine.AttackState.StopRushAttackAnimation();
        Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.ChasingState);
    }

    public void OnDie()
    {
        Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.IdleState);
        Skeleton.fieldOfVision.enabled = false;
        
        StartCoroutine(DestroySkeleton());
    }

    private IEnumerator DestroySkeleton()
    {
        yield return new WaitForSeconds(2f);
        
        Destroy(Skeleton.gameObject);
    }
}
