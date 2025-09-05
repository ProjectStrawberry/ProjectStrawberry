using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkull : MonoBehaviour
{
    public FloatingSkullStateMachine stateMachine;
    
    [field: SerializeField] public FloatingSkullSO StatData { get; private set; }
    [field:SerializeField] public FloatingSkullCondition Condition { get; private set; }
    public Animator Animator { get; private set; }

    public FloatingSkullAnimationHandler AnimationHandler { get; private set; }
    [field:SerializeField] public FloatingSkullAnimationData AnimationData { get; private set; }
    
    [Header("공격 관련")] 
    public CircleCollider2D fieldOfVision;
    public Player targetPlayer;
    public ProjectileHandler ProjectileHandler;
    
    [Header("SFX")]
    [SerializeField] public AudioClip projectileFireSFX;
    [SerializeField] public AudioClip damagedSFX;
    [SerializeField] public AudioClip deadSFX;

    private void Awake()
    {
        stateMachine = new FloatingSkullStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
        ProjectileHandler = GetComponentInChildren<ProjectileHandler>();
        AnimationHandler = GetComponentInChildren<FloatingSkullAnimationHandler>();
        Condition = GetComponent<FloatingSkullCondition>();
    }

    private void Start()
    {
        fieldOfVision.radius = StatData.playerDetectRange;
        // fieldOfVision.offset = new Vector2((StatData.playerDetectRange) / 2, 0);
        
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    
    private void Update()
    {
        stateMachine.Update();
        stateMachine.HandleInput();
    }
}
