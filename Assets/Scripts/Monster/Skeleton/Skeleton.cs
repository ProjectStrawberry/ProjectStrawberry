using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private SkeletonStateMachine stateMachine;
    
    [field:SerializeField] public SkeletonAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }
    
    [field:SerializeField] public SkeletonSO StatData { get; private set; }
    
    [Header("공격 관련 콜라이더")] 
    public Collider2D attackCollider;

    [Header("이동 관련")] 
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    [SerializeField] public Rigidbody2D rigidbody;
    public SpriteRenderer skeletonSprite;
    public event Action<Collision2D> OnCollide;

    private void Awake()
    {
        stateMachine = new SkeletonStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
        attackCollider.enabled = false;
        rigidbody = GetComponent<Rigidbody2D>();
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
}
