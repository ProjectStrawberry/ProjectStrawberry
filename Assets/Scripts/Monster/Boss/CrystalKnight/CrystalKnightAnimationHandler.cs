using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalKnightAnimationHandler : MonoBehaviour
{
    public CrystalKnight CrystalKnight;
    public SpriteRenderer Sprite;
    private Color originColor;
    private Color blinkColor;

    private bool isDashing = false;
    private bool isRushing = false;
    private bool isInvincible = false;
    private int isComboAttackFirst = 0;
    private float curTime = 0f;
    private float blinkTimer = 0f;
    private bool isBlink = false;

    [Header("공격 관련 수치들")] 
    [SerializeField] private Vector2 bossRoomMin;
    [SerializeField] private Vector2 bossRoomMax;
    [SerializeField] private float ComboAttackDashSpeed = 28f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private LayerMask stopLayer;
    [SerializeField] private float rushSpeed = 28f;
    [SerializeField] private float longProjectileTime = 6f;
    [SerializeField] private float projectileFireRate = 0.2f;
    [SerializeField] public List<Vector2> randPosList = new List<Vector2>();
    [SerializeField] private float moveDuration = 1.5f;
    
    [Header("공격 투사체 관련")]
    public GameObject thunderLaser;
    public GameObject beforeThunderLaser;
    public GameObject iceBall;
    public GameObject targetMiddle;
    [SerializeField] public List<ProjectileHandler> projectileHandlers = new List<ProjectileHandler>();
    [SerializeField] public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        CrystalKnight = GetComponentInParent<CrystalKnight>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CrystalKnight.GetComponent<CrystalKnightCondition>().onTakeDamage += BlinkSprite;
        
        originColor = Sprite.color;
        blinkColor = new Color(originColor.r, originColor.g, originColor.b, 0.3f); // 반투명
    }

    private void LateUpdate()
    {
        if (isInvincible)
        {
            curTime += Time.deltaTime;
            Debug.Log(curTime);
            Sprite.color = blinkColor;

            // 무적 시간 끝나면 종료
            if (curTime >= CrystalKnight.Condition.invincibleTime)
            {
                isInvincible = false;
                Sprite.color = originColor; // 원래 색상으로 복구
            }
        }
        
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

    public void StartLaserFire()
    {
        var randTime = (float)Random.Range(3, 11) / 10f;
        Debug.Log("랜덤 재생 시간: " + randTime);
        
        for (int i = 0; i < 3; i++)
        {
            var randPos = new Vector2(Random.Range(bossRoomMin.x, bossRoomMax.x), 0);

            var beforeThunder = Instantiate(beforeThunderLaser, randPos, Quaternion.identity);
            beforeThunder.GetComponent<BeforeThunderLaser>().Init(randTime, thunderLaser, CrystalKnight.StatData.damage);
        }

        StartCoroutine(EndLaserFire(randTime));
    }

    private IEnumerator EndLaserFire(float time)
    {
        Debug.Log("레이저 생성 이후 대기 시간: " + (time));
        yield return new WaitForSeconds(time);
        
        // 이후 Idle 상태로 돌아가 정해진 시간만큼 대기한다.
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.LaserFireParameterHash, false);
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }

    public IEnumerator MovetoRandomPos()
    {
        int randIndex = Random.Range(0, randPosList.Count);
        var startPos = CrystalKnight.transform.position;
        var targetPos = randPosList[randIndex];
        float curTime = 0f;

        while (curTime <= moveDuration)
        {
            curTime += Time.deltaTime;
            float t = curTime / moveDuration;
            
            CrystalKnight.transform.position = Vector2.Lerp(startPos, targetPos, t);
            
            yield return null;
        }

        CrystalKnight.transform.position = targetPos;
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.LongProjectileFireParameterHash, true);
    }
    
    public void StartLongProjectileFire()
    {
        StartCoroutine(LongProjectileFireCoroutine());
    }

    private IEnumerator LongProjectileFireCoroutine()
    {
        float curTime = 0f;
        float fireTime = 0f;
        float rotationSpeed = 360f / longProjectileTime;

        while (curTime <= longProjectileTime)
        {
            targetMiddle.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

            fireTime += Time.deltaTime;
            if (fireTime >= projectileFireRate)
            {
                fireTime = 0f;
                for (int i = 0; i < projectileHandlers.Count; i++)
                {
                    if (i < targets.Count)
                        projectileHandlers[i].Attack(targets[i]);
                }
            }
            
            curTime += Time.deltaTime;
            yield return null;
        }
    }

    public void EndLongProjectileFire()
    {
        // 이후 Idle 상태로 돌아가 정해진 시간만큼 대기한다.
        CrystalKnight.Animator.SetBool(CrystalKnight.AnimationData.LongProjectileFireParameterHash, false);
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.IdleState);
    }

    public void BlinkSprite()
    {
        isInvincible = true;
        curTime = 0f;
        blinkTimer = 0f;
        isBlink = true;
        Sprite.color = blinkColor;
    }

    public void StartDeadCoroutine()
    {
        StartCoroutine(DeadCoroutine());
    }
    
    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(2f);
        
        Destroy(CrystalKnight.gameObject);
    }
}
