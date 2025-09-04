using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossActionType
{
    Close,      // 근거리 공격 모두
    Long,       // 원거리 공격 모두
    Laser,      // 레이저 발사 공격만
    WaitShort,  // 3초 대기
    WaitLong    // 5초 대기
}

public class CrystalKnightAttackHandler : MonoBehaviour
{
    public CrystalKnight CrystalKnight;

    [Header("보스 공격 관련 수치들")] 
    [SerializeField] private float shortWaitingTime = 3f;
    [SerializeField] private float longWaitingTime = 5f;
    [SerializeField] private float damage;

    private void Awake()
    {
        CrystalKnight = GetComponentInParent<CrystalKnight>();

        damage = CrystalKnight.StatData.damage;
    }

    public void WaitForIdleState(BossActionType waitType)
    {
        var waitTime = (waitType == BossActionType.WaitShort) ? shortWaitingTime : longWaitingTime;

        StartCoroutine(WaitCoroutine(waitTime));
    }

    private IEnumerator WaitCoroutine(float time)
    {
        Debug.Log(CrystalKnight.name + "가 " + time + "초 동안 대기합니다.");
        yield return new WaitForSeconds(time);
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.AttackState);
    }
    
    public void RushAttack()
    {
        Debug.Log(CrystalKnight.name + "의 돌진 공격!");
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }

    public void ComboAttack()
    {
        Debug.Log(CrystalKnight.name + "의 연속 공격!");
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }
    
    public void LongProjectileFire()
    {
        Debug.Log(CrystalKnight.name + "의 구형 투사체 발사!");
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }
    
    public void LaserFire()
    {
        Debug.Log(CrystalKnight.name + "의 레이저 생성!");
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }
}
