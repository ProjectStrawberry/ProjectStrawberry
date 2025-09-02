using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int isMove = Animator.StringToHash("isMove");
    private static readonly int isJump = Animator.StringToHash("isJump");
    private static readonly int isDoubleJump = Animator.StringToHash("isDoubleJump");
    private static readonly int firstAttack = Animator.StringToHash("FirstAttack");
    private static readonly int secondAttack = Animator.StringToHash("SecondAttack");
    private static readonly int rangeAttack = Animator.StringToHash("RangeAttack");
    private static readonly int airAttackTrigger = Animator.StringToHash("airAttackTrigger");
    private static readonly int isHeal = Animator.StringToHash("isHeal");
    private static readonly int isDash = Animator.StringToHash("isDash");
    private static readonly int isDamaged = Animator.StringToHash("isDamaged");

    protected Animator animator;
    protected SpriteRenderer spriteRenderer; 

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Move(Vector2 obj)
    {
        if(obj.x<0f) spriteRenderer.flipX = true;
        else if (obj.x>0f)spriteRenderer.flipX = false;
        animator.SetBool(isMove, Mathf.Abs(obj.x)>0);
    }

    public void Jump(bool isTrue)
    {
        animator.SetBool(isJump, isTrue);
    }

    public void DoubleJump(bool isTrue)
    {
        animator.SetBool(isDoubleJump, isTrue);
    }

    public void Attack(int setCombo)
    {
        if (setCombo == 0)
            animator.Play(firstAttack, -1, 0f); 
        else if (setCombo == 1)
            animator.Play(secondAttack, -1, 0f);
    }
    public void AirAttack()
    {
        animator.SetTrigger(airAttackTrigger);
    }

    public void RangeAttack()
    {
        animator.Play(rangeAttack, -1, 0f);
    }

    public void Dash(bool isTrue)
    {
        animator.SetBool(isDash, isTrue);

    }

    public void Heal(bool isTrue)
    {
        animator.SetBool(isHeal, isTrue);
    }

    public void Damaged()
    {
        animator.SetBool(isDamaged, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(isDamaged, false);
    }
}
