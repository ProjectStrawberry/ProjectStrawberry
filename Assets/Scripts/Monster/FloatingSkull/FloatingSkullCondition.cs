using System;
using System.Collections;
using UnityEngine;

public class FloatingSkullCondition : MonoBehaviour, IDamagable
{
    public FloatingSkull FloatingSkull;
    
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;

    private bool isDead = false;

    private void Awake()
    {
        FloatingSkull = GetComponent<FloatingSkull>();

        maxHealth = FloatingSkull.StatData.health;
        curHealth = maxHealth;
        isDead = false;
    }

    public void GetDamage(int damage)
    {
        if (isDead) return;
        
        curHealth -= damage;
        SoundManager.PlayClip(FloatingSkull.damagedSFX, true);
        
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
        
        SoundManager.PlayClip(FloatingSkull.deadSFX, true);

        isDead = true;

        int layer = LayerMask.NameToLayer("EnemyCorpse");
        ChangeLayerRecursively(FloatingSkull.gameObject, layer);

        StartCoroutine(DestroyFloatingSkull());
    }

    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        
        foreach(Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }

    private IEnumerator DestroyFloatingSkull()
    {
        yield return new WaitForSeconds(1f);
        
        Destroy(FloatingSkull.gameObject);
    }
}
