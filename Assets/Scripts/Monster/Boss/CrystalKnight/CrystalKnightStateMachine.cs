using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightStateMachine : StateMachine
{
    public CrystalKnight CrystalKnight { get; private set; }
    
    public CrystalKnightIdleState IdleState { get; private set; }
    public CrystalKnightAttackState AttackState { get; private set; }

    private BossActionType[] actionCycle = new BossActionType[]
    {
        BossActionType.WaitShort,   // 첫 3초 대기 -> 사이클에는 1번 포함되고 이후에는 X
        BossActionType.Close,       // 근거리
        BossActionType.WaitShort,   // 3초 대기
        BossActionType.Close,       // 근거리
        BossActionType.WaitShort,   // 3초 대기
        BossActionType.Laser,       // 레이저
        BossActionType.WaitLong,    // 5초 대기
        BossActionType.Close,       // 근거리
        BossActionType.WaitShort,   // 3초 대기
        BossActionType.Close,       // 근거리
        BossActionType.WaitShort,   // 3초 대기
        BossActionType.Long,        // 원거리(레이저/투사체)
        BossActionType.WaitLong     // 5초 대기
    };

    public int cycleIndex { get; set; } = 0;

    public BossActionType GetCurrentAction() => actionCycle[cycleIndex];

    public CrystalKnightStateMachine(CrystalKnight crystalKnight)
    {
        this.CrystalKnight = crystalKnight;

        IdleState = new CrystalKnightIdleState(this);
        AttackState = new CrystalKnightAttackState(this);
    }

    public void PlusCycleIndex()
    {
        if (cycleIndex == actionCycle.Length - 1)
        {
            cycleIndex = 1;
        }
        else
        {
            cycleIndex++;
        }
    }
}
