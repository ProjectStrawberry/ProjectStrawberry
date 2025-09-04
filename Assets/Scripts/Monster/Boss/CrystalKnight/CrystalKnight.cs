using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnight : MonoBehaviour
{
    [field:SerializeField] public Animator Animator { get; private set; }
    [field:SerializeField] public CrystalKngihtAnimationData AnimationData { get; private set; }
    [field:SerializeField] public CrystalKnightSO StatData { get; private set; }
    public CrystalKnightCondition Condition { get; private set; }
    public CrystalKnightStateMachine StateMachine;
    [field:SerializeField] public CrystalKnightAttackHandler AttackHandler { get; private set; }
    [field:SerializeField] public CrystalKnightAnimationHandler AnimationHandler { get; private set; }
    [field:SerializeField] public CrystalKnightAttackHitBoxHandler AttackHitBoxHandler { get; private set; }
    [field:SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
        StateMachine = new CrystalKnightStateMachine(this);
        AttackHandler = GetComponent<CrystalKnightAttackHandler>();
        AnimationHandler = GetComponentInChildren<CrystalKnightAnimationHandler>();
        AttackHitBoxHandler = GetComponentInChildren<CrystalKnightAttackHitBoxHandler>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
        StateMachine.HandleInput();
    }
}
