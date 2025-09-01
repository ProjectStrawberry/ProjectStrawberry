using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _boxCollider;
    public LayerMask excludeMask;

    [SerializeField] private SpriteRenderer characterRenderer;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;

    protected StatHandler statHandler;


    protected bool isRangeAttack;
    private float timeSinceLastAttack = float.MaxValue;

    public float fallMultiplier = 2f;
    private Vector2 moveInput;
    public bool isGrounded;               // 바닥 체크
    public bool isJumping;

    private bool isDash;
    private float dashCoolTime;

    private float attackDelay = 0.0f;
    private int comboStep = 0; // 현재 콤보 단계 (0 = 대기, 1 = 첫번째 공격, 2 = 두번째 공격)
    private float comboResetTime = 1.0f; // 콤보를 이어갈 수 있는 시간
    private float comboTimer = 0f;

    private float airAttackCoolTime;

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

        if (!isDash) // >>> 추가된 코드 : 대쉬 중에는 이동 입력 무시
        {
            _rigidbody.velocity = new Vector2(Movement(moveInput), _rigidbody.velocity.y);
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
        if (collision.contacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Ground")) // 아래에서 충돌 시
        {
            isGrounded = true;
            isJumping = false;
            animationHandler.Jump(false);
            animationHandler.DoubleJump(false);
        }
    }


    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }


    public void Death()
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

    //public void OnMoveInput(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Performed)
    //    {
    //        movementDirection = context.ReadValue<Vector2>();
    //    }
    //    else if (context.phase == InputActionPhase.Canceled)
    //    {
    //        movementDirection = Vector2.zero;
    //    }
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) // 점프 시작
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

        if (context.started && isJumping) // 더블점프 시작
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
        if(context.started && isGrounded)
        {
            HandleComboAttack();
        }
        else if (context.started && !isGrounded && Time.time - airAttackCoolTime > 0.75f)
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

    }

    public void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        isRangeAttack = inputValue.isPressed;
    }

    public void Invincible()
    {
        StartCoroutine(DashCoroutine(statHandler.GetStat(StatType.DashDuration)));
    }

    IEnumerator DashCoroutine(float duration)
    {
        //_boxCollider.excludeLayers = excludeMask;
        //animationHandler.Dash(true);

        //yield return new WaitForSeconds(duration);
        //animationHandler.Dash(false);
        //_boxCollider.includeLayers = excludeMask;

        _boxCollider.excludeLayers = excludeMask;
        animationHandler.Dash(true);

        // >>> 추가된 코드 : 대쉬 시작
        isDash = true;
        float dashSpeed = statHandler.GetStat(StatType.DashDistance) / duration; // duration 동안 5유닛 이동하도록 속도 설정
        Vector2 dashDirection = characterRenderer.flipX ? Vector2.left : Vector2.right;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _rigidbody.velocity = new Vector2(dashDirection.x * dashSpeed, 0f); // X방향으로만 이동
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigidbody.velocity = Vector2.zero; // 대쉬 끝나면 속도 초기화
        isDash = false;
        // >>> 추가된 코드 끝

        animationHandler.Dash(false);
        _boxCollider.includeLayers = excludeMask;
    }

    private void HandleComboAttack()
    {
        if (comboStep == 0 && Time.time - attackDelay > 0.4) // 첫 번째 공격 시작
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 1;
            comboTimer = comboResetTime;
            animationHandler.Attack(0); // 첫 번째 공격 애니메이션
        }
        else if (comboStep == 1 && Time.time - attackDelay > 0.4) // 두 번째 공격
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 0;
            comboTimer = comboResetTime;
            animationHandler.Attack(1); // 두 번째 공격 애니메이션
        }
        else
        {
            // 이미 마지막 콤보 공격이라면 더 이상 이어지지 않음
        }
    }
    private void AirAttack()
    {
        animationHandler.AirAttack();
    }

    protected void Attack(int comboStep)
    {
        Debug.Log($"콤보 {comboStep}");
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }
}
