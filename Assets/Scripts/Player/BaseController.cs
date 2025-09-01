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
    public bool isGrounded;               // �ٴ� üũ
    public bool isJumping;

    private bool isDash;
    private float dashCoolTime;

    private float attackDelay = 0.0f;
    private int comboStep = 0; // ���� �޺� �ܰ� (0 = ���, 1 = ù��° ����, 2 = �ι�° ����)
    private float comboResetTime = 1.0f; // �޺��� �̾ �� �ִ� �ð�
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
        // �޺� Ÿ�̸� ����
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

        if (!isDash) // >>> �߰��� �ڵ� : �뽬 �߿��� �̵� �Է� ����
        {
            _rigidbody.velocity = new Vector2(Movement(moveInput), _rigidbody.velocity.y);
        }


        // --- ���� �ӵ� ���� (���ϰ� �� ������ �ϰ� �ʹٸ�) ---
        //if (_rigidbody.velocity.y < 0)
        //{
        //    _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        //}
    }

    private float Movement(Vector2 direction)
    {
        float targetSpeed = direction.x * statHandler.GetStat(StatType.Speed);
        //�˹� ���� �� ���==============================================================================
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
        if (collision.contacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Ground")) // �Ʒ����� �浹 ��
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
        if (context.performed && isGrounded) // ���� ����
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            Debug.Log(velocity);
            _rigidbody.velocity = velocity;

            animationHandler.Jump(true);
            isGrounded = false; // ���ÿ� (��¥�δ� Raycast ������ ����)
            isJumping = true;
        }

        if (context.canceled) // ���� ��ư�� ���� ��
        {
            if (_rigidbody.velocity.y > 0) // ���� ��� ���� ���� ����
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * statHandler.GetStat(StatType.CutJumpForceMultiplier));
            }
        }

        if (context.started && isJumping) // �������� ����
        {
            float gravity = -Physics2D.gravity.y * _rigidbody.gravityScale;
            float jumpVelocity = Mathf.Sqrt(2 * gravity * statHandler.GetStat(StatType.JumpeForce));

            Vector2 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            Debug.Log(velocity);
            _rigidbody.velocity = velocity;

            animationHandler.DoubleJump(true);
            isJumping = false; // ���ÿ� (��¥�δ� Raycast ������ ����)
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

        // >>> �߰��� �ڵ� : �뽬 ����
        isDash = true;
        float dashSpeed = statHandler.GetStat(StatType.DashDistance) / duration; // duration ���� 5���� �̵��ϵ��� �ӵ� ����
        Vector2 dashDirection = characterRenderer.flipX ? Vector2.left : Vector2.right;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _rigidbody.velocity = new Vector2(dashDirection.x * dashSpeed, 0f); // X�������θ� �̵�
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigidbody.velocity = Vector2.zero; // �뽬 ������ �ӵ� �ʱ�ȭ
        isDash = false;
        // >>> �߰��� �ڵ� ��

        animationHandler.Dash(false);
        _boxCollider.includeLayers = excludeMask;
    }

    private void HandleComboAttack()
    {
        if (comboStep == 0 && Time.time - attackDelay > 0.4) // ù ��° ���� ����
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 1;
            comboTimer = comboResetTime;
            animationHandler.Attack(0); // ù ��° ���� �ִϸ��̼�
        }
        else if (comboStep == 1 && Time.time - attackDelay > 0.4) // �� ��° ����
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 0;
            comboTimer = comboResetTime;
            animationHandler.Attack(1); // �� ��° ���� �ִϸ��̼�
        }
        else
        {
            // �̹� ������ �޺� �����̶�� �� �̻� �̾����� ����
        }
    }
    private void AirAttack()
    {
        animationHandler.AirAttack();
    }

    protected void Attack(int comboStep)
    {
        Debug.Log($"�޺� {comboStep}");
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }
}
