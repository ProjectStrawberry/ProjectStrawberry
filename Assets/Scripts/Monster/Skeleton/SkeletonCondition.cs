using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCondition : MonoBehaviour, IDamagable
{
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;

    public Skeleton Skeleton;
    
    private bool isDead = false;

    private void Awake()
    {
        Skeleton = GetComponent<Skeleton>();

        maxHealth = Skeleton.StatData.health;
        curHealth = maxHealth;
        isDead = false;
    }

    public void GetDamage(int damage)
    {
        if (isDead) return;
        
        curHealth -= damage;
        Debug.Log(Skeleton.name + " 데미지를 입음: " + damage);
        SoundManager.PlayClip(Skeleton.damagedSfx, true);
        
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
        isDead = true;
        Skeleton.fieldOfVision.enabled = false;
        SoundManager.PlayClip(Skeleton.deadSfx, true);

        int layer = LayerMask.NameToLayer("EnemyCorpse");
        ChangeLayerRecursively(Skeleton.gameObject, layer);
        
        Skeleton.Animator.SetTrigger(Skeleton.AnimationData.DieParameterHash);
    }
    
    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        
        foreach(Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }
}
