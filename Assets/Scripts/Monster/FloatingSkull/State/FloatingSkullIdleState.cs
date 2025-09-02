using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullIdleState : FloatingSkullBaseState
{
    public FloatingSkullIdleState(FloatingSkullStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.FloatingSkull.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.FloatingSkull.AnimationData.IdleParameterHash);
    }
}
