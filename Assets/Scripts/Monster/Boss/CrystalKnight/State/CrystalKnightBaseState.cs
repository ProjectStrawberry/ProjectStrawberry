using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightBaseState : IState
{
    protected CrystalKnightStateMachine stateMachine;

    public CrystalKnightBaseState(CrystalKnightStateMachine stateMachine)
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
        stateMachine.CrystalKnight.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.CrystalKnight.Animator.SetBool(animatorHash, false);
    }
}
