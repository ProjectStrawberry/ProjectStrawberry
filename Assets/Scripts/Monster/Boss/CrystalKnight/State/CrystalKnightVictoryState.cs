using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightVictoryState : CrystalKnightBaseState
{
    public CrystalKnightVictoryState(CrystalKnightStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("CrystalKnight Victory State 진입");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
