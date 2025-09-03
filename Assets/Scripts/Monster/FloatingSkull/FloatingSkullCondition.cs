using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullCondition : MonoBehaviour, IDamagable
{
    public FloatingSkull FloatingSkull;
    
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;

    private void Awake()
    {
        FloatingSkull = GetComponent<FloatingSkull>();

        maxHealth = FloatingSkull.StatData.health;
        curHealth = maxHealth;
    }

    public void GetDamage(int damage)
    {
        curHealth -= damage;
        Debug.Log(FloatingSkull.name + " 데미지를 입음: " + damage);
        
        if (curHealth <= 0)
        {
            Dead();
        }
        else
        {
            FloatingSkull.AnimationHandler.StartHurtAnimation();
        }
    }

    public void Dead()
    {
        FloatingSkull.Animator.SetTrigger(FloatingSkull.AnimationData.DieParameterHash);
        FloatingSkull.stateMachine.ChangeState(FloatingSkull.stateMachine.IdleState);
        FloatingSkull.fieldOfVision.enabled = false;

        StartCoroutine(DestroyFloatingSkull());
    }

    private IEnumerator DestroyFloatingSkull()
    {
        yield return new WaitForSeconds(1f);
        
        Destroy(FloatingSkull.gameObject);
    }
}
