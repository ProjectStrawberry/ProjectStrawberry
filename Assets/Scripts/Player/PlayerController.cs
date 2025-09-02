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


    protected bool isRangeAttack;

    public float fallMultiplier = 2f;
    private Vector2 moveInput;
    public bool isGrounded;               // �ٴ� üũ
    public bool isJumping;

    private bool isDash;
    private float dashCoolTime;

    private float attackDelay = 0.0f;
    private float rangeDelay = 0.0f;
    private int comboStep = 0; 
    private float comboResetTime = 1.0f; // �޺��� �̾ �� �ִ� �ð�
    private float comboTimer = 0f;

    private float airAttackCoolTime;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.0f;   // ���� �ݰ�
    [SerializeField] private Vector2 attackOffset = new Vector2(1f, 0f); // ĳ���� ���� ������


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
        if (collision.contacts[0].normal.y > 0.5f) // �Ʒ����� �浹 ��
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
        if (context.performed && isGrounded && !isDash) // ���� ����
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

        if (context.started && isJumping && !isDash) // �������� ����
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
            Debug.Log($"�� ����{Time.time}");
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
            animationHandler.Heal(false);
            CancelInvoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && Time.time - rangeDelay > statHandler.GetStat(StatType.RangeAttackDelay) && !isDash)
        {
            rangeDelay = Time.time;
            animationHandler.RangeAttack();
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

        // �뽬 �Ÿ�
        float dashDistance = statHandler.GetStat(StatType.DashDistance);
        Vector2 dashDirection = characterRenderer.flipX ? Vector2.left : Vector2.right;

        // ������ / ��ǥ��
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

        // ������ ��ġ ����
        _rigidbody.MovePosition(endPos);

        isDash = false;
        animationHandler.Dash(false);
        _boxCollider.includeLayers = excludeMask;
    }

    private void HandleComboAttack()
    {
        if (comboStep == 0 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay)) // ù ��° ���� ����
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 1;
            comboTimer = comboResetTime;
            animationHandler.Attack(0); // ù ��° ���� �ִϸ��̼�
        }
        else if (comboStep == 1 && Time.time - attackDelay > statHandler.GetStat(StatType.AttackDelay)) // �� ��° ����
        {
            attackDelay = Time.time;
            Attack(comboStep);
            comboStep = 0;
            comboTimer = comboResetTime;
            animationHandler.Attack(1); // �� ��° ���� �ִϸ��̼�
        }
        else
        {
        }
    }
    private void AirAttack()
    {
        animationHandler.AirAttack();
    }

    protected void Attack(int comboStep)
    {
        Debug.Log($"�޺� {comboStep} ���� �ߵ�");

        // ĳ���� �ٶ󺸴� ���� (flipX�� �¿� �Ǵ�)
        Vector2 attackDir = characterRenderer.flipX ? Vector2.left : Vector2.right;

        // ���� ��ġ (ĳ���� ��ġ + ������ * ����)
        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * attackDir.x, attackOffset.y);

        // �簢�� ũ�� (Inspector���� ���� ����)
        Vector2 attackSize = new Vector2(attackRange, attackRange); // ���� ���� ����

        // ���� ���� ���� �� Ž��
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyMask);

        foreach (Collider2D hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target != null)
            {
                int damage = (int)statHandler.GetStat(StatType.Attack);
                target.GetDamage(damage);
                Debug.Log($"{hit.name} ���� {damage} �������� ����");
            }
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }

    private void UseStemina()
    {
        playerCondition.UseStemina();
        Debug.Log("��� ����");
    }

    private void Heal()
    {
        
        animationHandler.Heal(false);
        playerCondition.Heal((int)statHandler.GetStat(StatType.HealAmount));
        Debug.Log($"�� �Ϸ� {Time.time}");
    }

    private void OnDrawGizmosSelected()
    {
        if (characterRenderer == null) return;

        Vector2 attackDir = characterRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * attackDir.x, attackOffset.y);

        Vector2 attackSize = new Vector2(attackRange, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }
}
