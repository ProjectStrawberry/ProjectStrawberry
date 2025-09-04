using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CrystalKnightAnimationHandler : MonoBehaviour
{
    public CrystalKnight CrystalKnight;

    private bool isDashing = false;
    private bool isRushing = false;
    private int isComboAttackFirst = 0;
    
    [Header("공격 관련 수치들")]
    [SerializeField] private float ComboAttackDashSpeed = 28f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private LayerMask stopLayer;
    [SerializeField] private float rushSpeed = 28f;

    private void Awake()
    {
        CrystalKnight = GetComponentInParent<CrystalKnight>();
    }

    public void StartComboAttack()
    {
        CrystalKnight.AttackHitBoxHandler.comboAttackCollider.enabled = true;
        StartCoroutine(MoveToPlayer());
        isComboAttackFirst++;
    }

    private IEnumerator MoveToPlayer()
    {
        isDashing = true;

        var player = PlayerManager.Instance.player.transform;
        var direction = (player.position - CrystalKnight.transform.position).normalized;

        CrystalKnight.Rigidbody.velocity = direction * ComboAttackDashSpeed;

        float elapsed = 0f;
        while (elapsed < dashDuration && isDashing)
        {
            // 플레이어 감지 (앞쪽 0.5 유닛, 반경 0.5)
            Collider2D hit = Physics2D.OverlapCircle(
                CrystalKnight.transform.position + direction * 0.5f, 
                0.5f, 
                stopLayer
            );

            if (hit != null) // 플레이어 발견 → 돌진 중단
            {
                Debug.Log("돌진 중 플레이어 감지 → 멈춤");
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        CrystalKnight.Rigidbody.velocity = Vector2.zero;
        isDashing = false;
    }
    
    public void TurnOffHitBox()
    {
        CrystalKnight.AttackHitBoxHandler.comboAttackCollider.enabled = false;
    }
    
    public void EndComboAttack()
    {
        var dist = Mathf.Abs(PlayerManager.Instance.player.transform.position.x - CrystalKnight.transform.position.x);

        if (dist <= 2)
        {
            // 0.8초후 다시 콤보 어택
            Debug.Log("유닛 2이내에 플레이어가 존재합니다.");
            if (isComboAttackFirst % 2 != 0)
            {
                Debug.Log("콤보 공격을 다시 실행합니다.");
                // 2번째 공격 발동
                StartCoroutine(ReStartComboAttack());
            }
            else
            {
                Debug.Log("콤보 공격을 중지합니다.");
                // 이후 Idle 상태로 돌아가 정해진 시간만큼 대기한다.
                CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.CloseAttackParameterHash, false);
                CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
            }
        }
        else
        {
            // 이후 Idle 상태로 돌아가 정해진 시간만큼 대기한다.
            CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.CloseAttackParameterHash, false);
            CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
        }
    }

    private IEnumerator ReStartComboAttack()
    {
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.IdleParameterHash, true);
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.CloseAttackParameterHash, false);
        
        yield return new WaitForSeconds(0.8f);
        
        CrystalKnight.AttackHandler.ComboAttack();
    }

    public void StartRushAttack()
    {
        CrystalKnight.AttackHitBoxHandler.rushAttackCollider.enabled = true;
        StartCoroutine(RushToPlayer());
    }

    private IEnumerator RushToPlayer()
    {
        isRushing = true;

        // 패턴 시작 시 플레이어 위치 저장
        Vector2 targetPos = PlayerManager.Instance.player.transform.position;
        Vector2 startPos = CrystalKnight.transform.position;
        Vector2 direction = (targetPos - startPos).normalized;

        // velocity로 돌진 시작
        CrystalKnight.Rigidbody.velocity = direction * rushSpeed;

        while (isRushing)
        {
            // 목표 위치를 지나쳤는지 체크
            float traveled = Vector2.Distance(startPos, CrystalKnight.transform.position);
            float targetDist = Vector2.Distance(startPos, targetPos);

            if (traveled >= targetDist + 2f)
            {
                Debug.Log("목표 지점 +2 유닛 도달 → 돌진 중단");
                break;
            }

            yield return null;
        }

        // 돌진 멈춤
        CrystalKnight.Rigidbody.velocity = Vector2.zero;
        isRushing = false;
    }

    public void EndRushAttack()
    {
        CrystalKnight.AttackHitBoxHandler.rushAttackCollider.enabled = false;
        
        // 이후 Idle 상태로 돌아가 정해진 시간만큼 대기한다.
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.RushAttackParameterHash, false);
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }
}
