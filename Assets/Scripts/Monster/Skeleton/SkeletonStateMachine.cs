using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateMachine : StateMachine
{
    public Skeleton Skeleton { get; private set; }
    
    // Skeleton 몬스터의 경우 기본 상태 Idle, 플레이어가 추적 거리내에 존재하는 Walk
    // 플레이어가 공격 거리 내에 존재하는 Attack 상태로 구성
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonWalkState WalkState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonChasingState ChasingState { get; private set; }
    
    public SkeletonStateMachine(Skeleton skeleton)
    {
        this.Skeleton = skeleton;
        
        IdleState = new SkeletonIdleState(this);
        WalkState = new SkeletonWalkState(this);
        AttackState = new SkeletonAttackState(this);
        ChasingState = new SkeletonChasingState(this);
    }

    public bool IsCurrentStateAttackState()
    {
        if (currentState == AttackState)
            return true;

        return false;
    }
    
    public void StartHurtAnimation()
    {
        if (currentState == null)
            return;
        
        Skeleton.Animator.Play("Skeleton_Hurt", 0, 0f);
    }
}