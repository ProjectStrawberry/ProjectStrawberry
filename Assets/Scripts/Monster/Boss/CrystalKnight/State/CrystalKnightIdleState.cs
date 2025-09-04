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
        
        ChooseWaitTime();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
        Debug.Log(stateMachine.CrystalKnight.name + " Idle 상태 퇴장");
    }

    private void ChooseWaitTime()
    {
        var action = stateMachine.GetCurrentAction();
        Debug.Log("Cycle Index: " + stateMachine.cycleIndex);
        Debug.Log("Action: " + action);
        if (action == BossActionType.WaitShort)
        {
            stateMachine.CrystalKnight.AttackHandler.WaitForIdleState(BossActionType.WaitShort);
            stateMachine.PlusCycleIndex();
        }
        else if (action == BossActionType.WaitLong)
        {
            stateMachine.CrystalKnight.AttackHandler.WaitForIdleState(BossActionType.WaitLong);
            stateMachine.PlusCycleIndex();
        }
        else
        {
            Debug.LogError("잘못된 BossActionType입니다: " + action);
        }
    }
}
