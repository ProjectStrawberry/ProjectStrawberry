using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightIdleState : CrystalKnightBaseState
{
    public CrystalKnightIdleState(CrystalKnightStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
        Debug.Log(stateMachine.CrystalKnight.name + " Idle 상태 입장");
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
        Debug.Log(stateMachine.CrystalKnight.name + " Idle 상태 퇴장");
    }

    private IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("3초 후");
    }
}
