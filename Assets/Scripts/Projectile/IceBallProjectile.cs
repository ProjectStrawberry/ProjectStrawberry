using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallProjectile : Projectile
{
    private Animator Animator;

    private bool isBurst = false;
    
    protected override void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
        Animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > projectileHandler.Duration)
        {
            DestroyProjectile();
        }
        
        if (!isBurst)
        {
            _rigidbody.velocity = direction * projectileHandler.Speed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    public override void Init(Vector2 direction, ProjectileHandler inpuProjectileHandler, ProjectileManager projectileManager)
    {
        base.Init(direction, inpuProjectileHandler, projectileManager);
        isBurst = false;
    }

    protected override void OnTriggerEnter2D(Collider2D hit)
    {
        if (groundLayer.value == (groundLayer.value | (1 << hit.gameObject.layer)))
        {
            if (hit.CompareTag("SemiSolid"))
            {
                return;
            }
            
            DestroyProjectile();
        }
        else if (hitLayer.value == (hitLayer.value | (1 << hit.gameObject.layer)))
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target != null)
            {
                int damage = projectileHandler.Power;

                target.GetDamage(damage);
            }
            
            DestroyProjectile();
        }
    }

    protected override void DestroyProjectile()
    {
        isBurst = true;
        
        _rigidbody.velocity = Vector2.zero;
        
        Animator.SetTrigger("Burst");
    }

    public void OnBurst()
    {
        OnDespawn();
    }
}
