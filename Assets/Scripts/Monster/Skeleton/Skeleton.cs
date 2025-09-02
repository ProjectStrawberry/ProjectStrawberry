using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public SkeletonStateMachine stateMachine;
    
    [field:SerializeField] public SkeletonAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }
    public SkeletonAnimationHandler AnimationHandler { get; private set; }
    [field:SerializeField] public SkeletonSO StatData { get; private set; }
    public PlayerController targetPlayer;

    [Header("공격 관련")] 
    public Collider2D fieldOfVision;
    public Collider2D attackCollider;
    public LayerMask playerLayer;
    public SkeletonAttackColliderHandler AttackColliderHandler;

    [Header("이동 관련")] 
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    [SerializeField] public Rigidbody2D rigidbody;
    public SpriteRenderer skeletonSprite;
    public event Action<Collision2D> OnCollide;
    public event Action<Collision2D> OnCollideExit;

    private void Awake()
    {
        stateMachine = new SkeletonStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        AnimationHandler = GetComponentInChildren<SkeletonAnimationHandler>();
        AnimationData.Initialize();
        attackCollider.enabled = false;
        rigidbody = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        AttackColliderHandler = GetComponentInChildren<SkeletonAttackColliderHandler>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.Update();
        stateMachine.HandleInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollide?.Invoke(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        OnCollideExit?.Invoke(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !stateMachine.IsCurrentStateAttackState())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    public void RushAttackMove()
    {
        Debug.Log("돌진합니다");
        StartCoroutine(RushCoroutine());
        // var dir = stateMachine.Skeleton.transform.localScale.x > 0 ? 1 : -1;
        //
        // rigidbody.velocity = new Vector2(dir * StatData.rushSpeed * 5, rigidbody.velocity.y);
    }

    private IEnumerator RushCoroutine()
    {
        var dir = stateMachine.Skeleton.transform.localScale.x > 0 ? 1 : -1;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(dir, 0, 0) * StatData.rushDistance;
        
        while (Vector3.Distance(transform.position, startPos) < StatData.rushDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 10 * StatData.rushSpeed * Time.deltaTime);
            
            RaycastHit2D hitObstacle = Physics2D.Raycast(groundCheck.position + Vector3.up * 0.5f, 
                Vector3.right * Mathf.Sign(transform.localScale.x)
                , groundCheckDistance, playerLayer);
            if (hitObstacle.collider != null)
            {
                break;
            }
            
            yield return null; // 다음 프레임까지 대기
        }
    }
}
