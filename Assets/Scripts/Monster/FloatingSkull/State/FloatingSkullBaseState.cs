using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullBaseState : IState
{
    protected FloatingSkullStateMachine stateMachine;
    
    public FloatingSkullBaseState(FloatingSkullStateMachine stateMachine)
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
        throw new System.NotImplementedException();
    }
    
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.FloatingSkull.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.FloatingSkull.Animator.SetBool(animatorHash, false);
    }
}
