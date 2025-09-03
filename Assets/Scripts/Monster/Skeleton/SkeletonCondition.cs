using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCondition : MonoBehaviour, IDamagable
{
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;

    public Skeleton Skeleton;

    private void Awake()
    {
        Skeleton = GetComponent<Skeleton>();

        maxHealth = Skeleton.StatData.health;
        curHealth = maxHealth;
    }

    public void GetDamage(int damage)
    {
        curHealth -= damage;
        Debug.Log(Skeleton.name + " 데미지를 입음: " + damage);
        
        if (curHealth <= 0)
        {
            Dead();
        }
        else
        {
            Skeleton.stateMachine.StartHurtAnimation();
        }
    }

    public void Dead()
    {
        Skeleton.Animator.SetTrigger(Skeleton.AnimationData.DieParameterHash);
    }
}
