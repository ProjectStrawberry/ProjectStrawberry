using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _boxCollider;

    [Header("레이어 마스크 설정")]
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private LayerMask excludeMask;
    [SerializeField] private LayerMask enemyMask;

    [Header("캐릭터 스프라이트")]
    [SerializeField] private SpriteRenderer characterRenderer;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    protected PlayerCondition playerCondition;
    protected ProjectileHandler projectileHandler;


    protected bool isRangeAttack;

    private Vector2 moveInput;
    private bool isGrounded;               // 바닥 체크
    private bool isJumping;
    private bool isLanding;
    private bool isDash;
    private bool isHeal;
    private bool isDamaged;
    private bool isUpDown;

    [HideInInspector]
    public bool canDoubleJump = false;

    private float dashCoolTime;

    private Vector2 targetVector;
    private Vector2 adjustVector = new Vector2(0, 1);

    private bool isAttacking = false;
    private float attackDelay = 0.0f;
    private float rangeDelay = 0.0f;
    private int comboStep = 0;
    private float comboTimer = 0f;

    private float airAttackCoolTime;

    [Header("근접 공격 범위")]
    [SerializeField] private float attackRange = 1.0f;   // 공격 반경
    [SerializeField] private Vector2 attackOffset = new Vector2(1f, 0f); // 캐릭터 기준 오프셋

    [Header("공중 공격 범위")]
    [SerializeField] private float airAttackRange = 1.0f;   // 공격 반경
    [SerializeField] private Vector2 airAttackOffset = new Vector2(1f, 0f); // 캐릭터 기준 오프셋

    [SerializeField] private GameObject target;

    [Header("힐, 대시 소리")]
    [SerializeField] private AudioClip healChanneling;
    [SerializeField] private AudioClip healCompleteClip;
    [SerializeField] private AudioClip dashClip;

    private GameObject healClip;

    [Header("힐 파티클")]
    [SerializeField] private ParticleSystem healParticle;

    [HideInInspector]
    public CinemachineVirtualCamera vcam;
    private float vcamOriginSize;
    private Coroutine heailingZoomIn;
    private float zoomMultiplier = 0.2f;

    private PlatformEffector2D platformEffector;
    private bool readyDownJump;

    private int attackCountForStemina;



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

        _boxCollider.excludeLayers = enemyMask;

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

        if (!isDash || !isDamaged) // 대쉬 중에는 이동 무시
        {
            float moveSpeed = Movement(moveInput);
            if (isHeal || isDamaged) moveSpeed = 0;
            // 공격 중에는 속도 1/4로 감소
            if (isAttacking)
            {
                moveSpeed *= statHandler.GetStat(StatType.AttackingSlow);
            }

            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
        }

        if(_rigidbody.velocity.y < 0 && isGrounded && !isLanding)
        {
            animationHandler.HoldJumpLastFrame();
        }

    }

    private float Movement(Vector2 direction)
    {
        float targetSpeed = direction.x * statHandler.GetStat(StatType.Speed);

        animationHandler.Move(direction);
        targetVector = direction + adjustVector;
        if (direction.x != 0 && !isUpDown) target.transform.localPosition = targetVector;
        return targetSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((GroundMask & (1 << collision.gameObject.layer)) != 0) isLanding = true;
        animationHandler.StartSpeed();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((GroundMask & (1 << collision.gameObject.layer)) != 0) isLanding = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GroundMask & (1 << collision.gameObject.layer)) != 0)
        {
            platformEffector = collision.GetComponent<PlatformEffector2D>();
            Debug.Log(_rigidbody.velocity.y);
            if (_rigidbody.velocity.y <= 2.7f)
            {
                animationHandler.StartSpeed();
                isGrounded = true;
                isJumping = false;
                animationHandler.Jump(false);
                animationHandler.DoubleJump(false);
            }
        }
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

    }
    //gameManager.GameOver();

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isJumping && !isDash && !isHeal && canDoubleJump && !readyDownJump) // 더블점프 시작
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            _rigidbody.velocity = velocity;

            animationHandler.DoubleJump(true);
            isJumping = false; //
        }

        if (context.performed && isGrounded && !isDash && !isHeal && !readyDownJump) // 점프 시작
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            _rigidbody.velocity = velocity;

            animationHandler.Jump(true);
            isGrounded = false; //
            isJumping = true;
        }

        if (context.canceled) // 점프 버튼을 뗐을 때
        {
            if (_rigidbody.velocity.y > 0) // 아직 상승 중일 때만 끊기
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * statHandler.GetStat(StatType.CutJumpForceMultiplier));
            }
        }

        if(context.started && readyDownJump)
        {
            if(platformEffector != null)
            {
                StartCoroutine(DownJump());
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isDash && !isHeal)
        {
            HandleComboAttack();
        }
        else if (context.started && !isGrounded && Time.time - airAttackCoolTime > statHandler.GetStat(StatType.AttackDelay) && !isDash && !isHeal)
        {
            airAttackCoolTime = Time.time;
            AirAttack();
            animationHandler.Jump(false);
            animationHandler.DoubleJump(false);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && Time.time - dashCoolTime > statHandler.GetStat(StatType.DashCoolTime) && !isHeal)
        {
            dashCoolTime = Time.time;
            Dash();
        }

    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isDash)
        {
            animationHandler.Heal(true);

            vcamOriginSize = vcam.m_Lens.OrthographicSize;
            heailingZoomIn = StartCoroutine(HealingZoomInCoroutine());

            healClip = SoundManager.PlayClip(healChanneling, true);

            isHeal = true;

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
            isHeal = false;
            CancelHeal();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && Time.time - rangeDelay > statHandler.GetStat(StatType.RangeAttackDelay) && !isDash && !isHeal)
        {
            playerCondition.UseStemina();
            rangeDelay = Time.time;
            animationHandler.RangeAttack();
            projectileHandler.Attack(target);
            //StartCoroutine(TestCoroutine());
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isUpDown = true;
            target.transform.localPosition = Vector2.up + adjustVector;
        }
        if (context.canceled)
        {
            isUpDown = false;
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if(context.performed && !isGrounded)
        {
            isUpDown = true;
            target.transform.localPosition = Vector3.down;
        }
        if(context.performed && isLanding)
        {
            isGrounded = false;
            readyDownJump = true;
            
        }
        if (context.canceled)
        {
            isUpDown = false;
            readyDownJump = false;
        }
        if (context.canceled && isLanding)
        {
            isGrounded = true;
            
        }
    }

    public void Dash()
    {
        if(dashClip != null) SoundManager.PlayClip(dashClip);
        StartCoroutine(DashCoroutine(statHandler.GetStat(StatType.DashDuration)));
    }

    IEnumerator DashCoroutine(float duration)
    {
        _boxCollider.excludeLayers = excludeMask;
        animationHandler.Dash(true);

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);

        float originScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;

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

        _rigidbody.gravityScale = originScale;
        isDash = false;
        animationHandler.Dash(false);
        if (!isGrounded)
        {
            animationHandler.HoldJumpLastFrame();
        }
        _boxCollider.includeLayers = excludeMask;
    }

    private void HandleComboAttack()
    {
        if (comboStep == 0 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay))
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 1;
            comboTimer = statHandler.GetStat(StatType.ComboResetTime);
            animationHandler.Attack(0);

            StartCoroutine(AttackSlowCoroutine()); // 이동속도 감소
        }
        else if (comboStep == 1 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay))
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 0;
            comboTimer = statHandler.GetStat(StatType.ComboResetTime);
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
                if (comboStep == 0) damage = (int)statHandler.GetStat(StatType.FirstAttack);
                else if (comboStep == 1) damage = (int)statHandler.GetStat(StatType.SecondAttack);

                target.GetDamage(damage);
                Debug.Log($"{hit.name} 에게 {damage} 데미지를 입힘");
                attackCountForStemina += 1;
                Debug.Log($"{hit.name} 에게 {damage} 데미지를 입힘");
                CheckAttackCountForStemina();
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

    private void CheckAttackCountForStemina()
    {
        if(attackCountForStemina >= statHandler.GetStat(StatType.RequireAttackForStemina))
        {
            attackCountForStemina = 0;
            playerCondition.RestoreStemina();
        }
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
        Destroy(healClip);
        if(heailingZoomIn != null) StopCoroutine(heailingZoomIn);
        vcam.m_Lens.OrthographicSize = vcamOriginSize;
        animationHandler.Heal(false);
        CancelInvoke(); // 예약된 힐/스태미나 소모 취소
    }

    private void Heal()
    {
        if (heailingZoomIn != null) StopCoroutine(heailingZoomIn);
        vcam.m_Lens.OrthographicSize = vcamOriginSize;
        if(healCompleteClip != null) SoundManager.PlayClip(healCompleteClip);
        isHeal = false;
        healParticle.Play();
        animationHandler.Heal(false);
        playerCondition.Heal((int)statHandler.GetStat(StatType.HealAmount));
        Debug.Log($"힐 완료 {Time.time}");
    }

    private IEnumerator HealingZoomInCoroutine()
    {
        while (true)
        {
            vcam.m_Lens.OrthographicSize -= Time.deltaTime*zoomMultiplier;
            yield return null;
        }
        
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

    private IEnumerator TestCoroutine()
    {
        for(int i = 0; i < 10; i++)
        {
            projectileHandler.Attack(target);
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public IEnumerator Damaged()
    {
        isDamaged = true;
        animationHandler.playDamaged();

        yield return new WaitForSeconds(statHandler.GetStat(StatType.DamagedAnimatingDuration));

        isDamaged = false;
        animationHandler.stopDamaged();

    }
    public IEnumerator Invincible()
    {
        _boxCollider.excludeLayers += excludeMask;
        
        yield return new WaitForSeconds(statHandler.GetStat(StatType.DamagedInvincibleDuration));

        _boxCollider.excludeLayers -= excludeMask;
    } 

    public IEnumerator DownJump()
    {
        platformEffector.surfaceArc = 0;
        PlatformEffector2D originEffector = platformEffector;

        yield return new WaitForSeconds(0.4f);

        originEffector.surfaceArc = 180;
    }
   
}
