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
        Debug.Log("근접 공격 실행 메서드");
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("근접 공격 끝!");
        Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.ChasingState);
    }
}
