using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightIdleState : CrystalKnightBaseState
{
    private bool isActed = false;
    
    public CrystalKnightIdleState(CrystalKnightStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log(stateMachine.CrystalKnight.name + " Idle 상태 입장");
        isActed = false;

        // if (!stateMachine.isDead)
        // {
        //     StartAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
        //     ChooseWaitTime();
        // }
        // else
        // {
        //     stateMachine.ChangeState(stateMachine.DeathState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
        Debug.Log(stateMachine.CrystalKnight.name + " Idle 상태 퇴장");
        isActed = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.TargetPlayer.playerCondition._currHealth <= 0)
        {
            return;
        }
        
        if (isActed == false)
        {
            if (!stateMachine.isDead)
            {
                StartAnimation(stateMachine.CrystalKnight.AnimationData.IdleParameterHash);
                ChooseWaitTime();
            }
            else
            {
                stateMachine.ChangeState(stateMachine.DeathState);
            }

            isActed = true;
        }
    }

    private void ChooseWaitTime()
    {
        var action = stateMachine.GetCurrentAction();
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
