using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightAttackState : CrystalKnightBaseState
{
    public CrystalKnightAttackState(CrystalKnightStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log(stateMachine.CrystalKnight.name + " Attack 상태 입장");

        if (!stateMachine.isDead)
        {
            ChooseBossAction();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.DeathState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(stateMachine.CrystalKnight.name + " Attack 상태 퇴장");
    }

    private void ChooseBossAction()
    {
        Debug.Log("Cycle Index: " + stateMachine.cycleIndex);
        var action = stateMachine.GetCurrentAction();
        Debug.Log("Action: " + action);
        switch (action)
        {
            case BossActionType.Close:
                ChooseRandomCloseAttack(action);
                break;
            case BossActionType.Long:
                ChooseRandomLongAttack(action);
                break;
            case BossActionType.Laser:
                stateMachine.PlusCycleIndex();
                stateMachine.CrystalKnight.AttackHandler.LaserFire();
                break;
            default:
                Debug.LogError("잘못된 BossActionType입니다: " + action);
                break;
        }
    }

    private void ChooseRandomLongAttack(BossActionType action)
    {
        if (action != BossActionType.Long)
            return;

        var rand = Random.Range(0f, 2f);
        if (rand == 0)
        {
            stateMachine.PlusCycleIndex();
            stateMachine.CrystalKnight.AttackHandler.LaserFire();
        }
        else
        {
            stateMachine.PlusCycleIndex();
            stateMachine.CrystalKnight.AttackHandler.LongProjectileFire();
        }
    }
    
    private void ChooseRandomCloseAttack(BossActionType action)
    {
        if (action != BossActionType.Close)
            return;

        var rand = Random.Range(0, 2);
        if (rand == 0)
        {
            stateMachine.PlusCycleIndex();
            stateMachine.CrystalKnight.AttackHandler.RushAttack();
        }
        else
        {
            stateMachine.PlusCycleIndex();
            stateMachine.CrystalKnight.AttackHandler.ComboAttack();
        }
    }
}
