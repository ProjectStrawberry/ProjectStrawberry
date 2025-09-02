﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private LayerMask excludeMask;
    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private SpriteRenderer characterRenderer;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    protected PlayerCondition playerCondition;
    protected ProjectileHandler projectileHandler;


    protected bool isRangeAttack;

    public float fallMultiplier = 2f;
    private Vector2 moveInput;
    public bool isGrounded;               // 바닥 체크
    public bool isJumping;

    private bool isDash;
    private float dashCoolTime;

    private bool isAttacking = false;
    private float attackDelay = 0.0f;
    private float rangeDelay = 0.0f;
    private int comboStep = 0; 
    private float comboResetTime = 0.5f; // 콤보를 이어갈 수 있는 시간
    private float comboTimer = 0f;

    private float airAttackCoolTime;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.0f;   // 공격 반경
    [SerializeField] private Vector2 attackOffset = new Vector2(1f, 0f); // 캐릭터 기준 오프셋

    [Header("Air Attack Settings")]
    [SerializeField] private float airAttackRange = 1.0f;   // 공격 반경
    [SerializeField] private Vector2 airAttackOffset = new Vector2(1f, 0f); // 캐릭터 기준 오프셋

    [SerializeField] private GameObject target;




    //private GameManager gameManager;

    //public void Init(GameManager gameManager)
    //{
    //    this.gameManager = gameManager;
    //}

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();
        playerCondition = GetComponent<PlayerCondition>();
        projectileHandler = GetComponentInChildren<ProjectileHandler>();

    }

    protected void Update()
    {
        // 콤보 타이머 감소
        if (comboStep > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    protected void FixedUpdate()
    {

        if (!isDash) // 대쉬 중에는 이동 무시
        {
            float moveSpeed = Movement(moveInput);

            // 공격 중에는 속도 1/4로 감소
            if (isAttacking)
            {
                moveSpeed *= statHandler.GetStat(StatType.AttackingSlow);
            }

            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
        }


        // --- 낙하 속도 보정 (낙하가 더 빠르게 하고 싶다면) ---
        //if (_rigidbody.velocity.y < 0)
        //{
        //    _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        //}
    }

    private float Movement(Vector2 direction)
    {
        float targetSpeed = direction.x * statHandler.GetStat(StatType.Speed);
        //넉백 구현 시 사용==============================================================================
        //if (knockbackDuration > 0.0f)
        //{
        //    direction *= 0.2f;
        //    direction += knockback;
        //}

        animationHandler.Move(direction);
        return targetSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) // 아래에서 충돌 시
        {
            if((GroundMask & (1 << collision.gameObject.layer)) != 0)
            {
                isGrounded = true;
                isJumping = false;
                animationHandler.Jump(false);
                animationHandler.DoubleJump(false);
            }
        }
    }


    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }


    public void Dead()
    {
        _rigidbody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
    //gameManager.GameOver();

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isDash) // 점프 시작
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            Debug.Log(velocity);
            _rigidbody.velocity = velocity;

            animationHandler.Jump(true);
            isGrounded = false; // 예시용 (진짜로는 Raycast 등으로 갱신)
            isJumping = true;
        }

        if (context.canceled) // 점프 버튼을 뗐을 때
        {
            if (_rigidbody.velocity.y > 0) // 아직 상승 중일 때만 끊기
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * statHandler.GetStat(StatType.CutJumpForceMultiplier));
            }
        }

        if (context.started && isJumping && !isDash) // 더블점프 시작
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            Debug.Log(velocity);
            _rigidbody.velocity = velocity;

            animationHandler.DoubleJump(true);
            isJumping = false; // 예시용 (진짜로는 Raycast 등으로 갱신)
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && isGrounded && !isDash)
        {
            HandleComboAttack();
        }
        else if (context.started && !isGrounded && Time.time - airAttackCoolTime > 0.75f && !isDash)
        {
            airAttackCoolTime = Time.time;
            AirAttack();
            animationHandler.Jump(false);
            animationHandler.DoubleJump(false);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.started && Time.time-dashCoolTime > statHandler.GetStat(StatType.DashCoolTime))
        {
            dashCoolTime = Time.time;
            Invincible();
        }
        
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            Debug.Log($"힐 시작 {Time.time}");
            animationHandler.Heal(true);

            float healDelay = statHandler.GetStat(StatType.HealDelay);
            float interval = statHandler.GetStat(StatType.HealSteminaConsumeInterval);

            for (float i = interval; i <= healDelay; i += interval)
            {
                Invoke("UseStemina", i);
            }

            Invoke("Heal", healDelay);
        }

        if (context.canceled)
        {
            CancelHeal();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && Time.time - rangeDelay > statHandler.GetStat(StatType.RangeAttackDelay) && !isDash)
        {
            rangeDelay = Time.time;
            animationHandler.RangeAttack();
            projectileHandler.Attack(target);
        }
    }

    public void Invincible()
    {
        StartCoroutine(DashCoroutine(statHandler.GetStat(StatType.DashDuration)));
    }

    IEnumerator DashCoroutine(float duration)
    {
        _boxCollider.excludeLayers = excludeMask;
        animationHandler.Dash(true);

        isDash = true;

        // 대쉬 거리
        float dashDistance = statHandler.GetStat(StatType.DashDistance);
        Vector2 dashDirection = characterRenderer.flipX ? Vector2.left : Vector2.right;

        // 시작점 / 목표점
        Vector2 startPos = _rigidbody.position;
        Vector2 endPos = startPos + dashDirection * dashDistance;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector2 newPos = Vector2.Lerp(startPos, endPos, smoothT);
            _rigidbody.MovePosition(newPos);

            yield return null;
        }

        // 마지막 위치 보정
        _rigidbody.MovePosition(endPos);

        isDash = false;
        animationHandler.Dash(false);
        _boxCollider.includeLayers = excludeMask;
    }

    private void HandleComboAttack()
    {
        if (comboStep == 0 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay))
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 1;
            comboTimer = comboResetTime;
            animationHandler.Attack(0);

            StartCoroutine(AttackSlowCoroutine()); // 이동속도 감소
        }
        else if (comboStep == 1 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay))
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 0;
            comboTimer = comboResetTime;
            animationHandler.Attack(1);

            StartCoroutine(AttackSlowCoroutine()); // 이동속도 감소
        }
    }
    private void AirAttack()
    {
        animationHandler.AirAttack();

        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x, attackOffset.y);

        Vector2 attackSize = new Vector2(attackRange, attackRange); // 가로 세로 비율

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyMask);

        foreach (Collider2D hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target != null)
            {
                int damage = (int)statHandler.GetStat(StatType.FirstAttack);

                target.GetDamage(damage);
                Debug.Log($"{hit.name} 에게 {damage} 데미지를 입힘");
            }
        }
    }

    protected void Attack(int comboStep)
    {
        Debug.Log($"콤보 {comboStep} 공격 발동");

        Vector2 attackDir = characterRenderer.flipX ? Vector2.left : Vector2.right;

        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * attackDir.x, attackOffset.y);

        Vector2 attackSize = new Vector2(attackRange, attackRange); // 가로 세로 비율

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyMask);

        foreach (Collider2D hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target != null)
            {
                int damage = 0;
                if(comboStep == 0) damage = (int)statHandler.GetStat(StatType.FirstAttack);
                else if (comboStep == 1) damage = (int)statHandler.GetStat(StatType.SecondAttack);

                target.GetDamage(damage);
                Debug.Log($"{hit.name} 에게 {damage} 데미지를 입힘");
            }
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }

    private IEnumerator AttackSlowCoroutine()
    {
        isAttacking = true;

        // 공격 딜레이 시간 동안만 속도 감소
        yield return new WaitForSeconds(statHandler.GetStat(StatType.AttackDelay));

        isAttacking = false;
    }

    private void UseStemina()
    {
        // 스태미나 사용 실패 시 힐 취소
        if (!playerCondition.UseStemina())
        {
            Debug.Log("스태미나 부족! 힐 취소됨");
            CancelHeal();
            return;
        }

        Debug.Log("기력 깎임");
    }

    private void CancelHeal()
    {
        animationHandler.Heal(false);
        CancelInvoke(); // 예약된 힐/스태미나 소모 취소
    }

    private void Heal()
    {
        
        animationHandler.Heal(false);
        playerCondition.Heal((int)statHandler.GetStat(StatType.HealAmount));
        Debug.Log($"힐 완료 {Time.time}");
    }

    private void OnDrawGizmosSelected()
    {
        if (characterRenderer == null) return;

        Vector2 attackDir = characterRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * attackDir.x, attackOffset.y);

        Vector2 attackSize = new Vector2(attackRange, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos, attackSize);

        Vector2 airAttackPos = (Vector2)transform.position + new Vector2(airAttackOffset.x, airAttackOffset.y);

        Vector2 airAttackSize = new Vector2(airAttackRange, airAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(airAttackPos, airAttackSize);
    }
}
