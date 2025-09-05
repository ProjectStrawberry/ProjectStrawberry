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
        yield return new WaitForSeconds(time);
        
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.AttackState);
    }
    
    public void RushAttack()
    {
        // 실제 공격
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.RushAttackParameterHash, true);
    }

    public void ComboAttack()
    {
        FaceToPlayer();
        
        // 실제 공격
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.CloseAttackParameterHash, true);
    }
    
    public void LongProjectileFire()
    {
        StartCoroutine(CrystalKnight.AnimationHandler.MovetoRandomPos());
    }
    
    public void LaserFire()
    {
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.LaserFireParameterHash, true);
    }

    private void FaceToPlayer()
    {
        var dir = Mathf.Sign(PlayerManager.Instance.player.gameObject.transform.position.x
                             - CrystalKnight.transform.position.x);
        
        CrystalKnight.transform.localScale = new Vector3(-dir, 1, 1);
    }
}
