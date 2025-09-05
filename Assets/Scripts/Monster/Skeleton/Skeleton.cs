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
    public Player targetPlayer;
    public SkeletonCondition skeletonCondition;

    [Header("공격 관련")] 
    public Collider2D fieldOfVision;
    public Collider2D attackCollider;
    public LayerMask playerLayer;
    public SkeletonAttackColliderHandler AttackColliderHandler;
    public Collider2D hitbox;

    [Header("이동 관련")] 
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public LayerMask rushStopLayer;
    [SerializeField] public Rigidbody2D _rigidbody;
    public SpriteRenderer skeletonSprite;
    public event Action<Collision2D> OnCollide;
    public event Action<Collision2D> OnCollideExit;

    [Header("SFX")] 
    [SerializeField] public AudioClip closeAttackSfx;
    [SerializeField] public AudioClip damagedSfx;
    [SerializeField] public AudioClip deadSfx;

    private void Awake()
    {
        stateMachine = new SkeletonStateMachine(this);
        skeletonCondition = GetComponent<SkeletonCondition>();
        Animator = GetComponentInChildren<Animator>();
        AnimationHandler = GetComponentInChildren<SkeletonAnimationHandler>();
        AnimationData.Initialize();
        attackCollider.enabled = false;
        _rigidbody = GetComponent<Rigidbody2D>();
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

    public void RushAttackMove()
    {
        Debug.Log("돌진합니다");
        StartCoroutine(RushCoroutine());
    }

    private IEnumerator RushCoroutine()
    {
        var dir = stateMachine.Skeleton.transform.localScale.x > 0 ? 1 : -1;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(dir, 0, 0) * StatData.rushDistance;
        SoundManager.PlayClip(closeAttackSfx, true);
        
        while (Vector3.Distance(transform.position, startPos) < StatData.rushDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 10 * StatData.rushSpeed * Time.deltaTime);
            
            // 앞에 플레이어가 있는지 체크
            RaycastHit2D hitObstacle = Physics2D.Raycast(groundCheck.position + Vector3.up * 0.5f, 
                Vector3.right * Mathf.Sign(transform.localScale.x)
                , groundCheckDistance, playerLayer);
            
            // 발 앞에 땅이 없는지 체크
            RaycastHit2D hit = Physics2D.Raycast(stateMachine.Skeleton.groundCheck.position, Vector3.down,
                stateMachine.Skeleton.groundCheckDistance, stateMachine.Skeleton.groundLayer);
            
            if (hitObstacle.collider != null || hit.collider == null)
            {
                Debug.Log("더는 돌진하지 못합니다!");
                break;
            }
            
            yield return null;
        }
    }
}
