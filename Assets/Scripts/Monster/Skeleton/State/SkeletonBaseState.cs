using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBaseState : IState
{
    protected SkeletonStateMachine stateMachine;
    
    public SkeletonBaseState(SkeletonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }
    
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Skeleton.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Skeleton.Animator.SetBool(animatorHash, false);
    }

    protected bool IsInDefaultAttackRange()
    {
        var dist = Mathf.Abs(stateMachine.Skeleton.transform.position.x -
                             PlayerManager.Instance.player.transform.position.x);

        if (dist <= stateMachine.Skeleton.StatData.DefaultAttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool IsInRushAttackRange()
    {
        var dist = Mathf.Abs(stateMachine.Skeleton.transform.position.x -
                             PlayerManager.Instance.player.transform.position.x);

        if (dist <= stateMachine.Skeleton.StatData.RushAttackRange && dist > stateMachine.Skeleton.StatData.DefaultAttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}