using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightDeathState : CrystalKnightBaseState
{
    public CrystalKnightDeathState(CrystalKnightStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Death 상태 입장");
        stateMachine.isDead = true;
        
        stateMachine.CrystalKnight.Animator.SetTrigger(stateMachine.CrystalKnight.AnimationData.DeadParameterHash);
    }
}
