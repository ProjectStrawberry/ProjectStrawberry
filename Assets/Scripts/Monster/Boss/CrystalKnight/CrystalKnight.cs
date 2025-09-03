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

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
        StateMachine = new CrystalKnightStateMachine(this);
        AttackHandler = GetComponent<CrystalKnightAttackHandler>();
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
