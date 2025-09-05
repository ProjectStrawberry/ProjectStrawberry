using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected LayerMask hitLayer;
    [SerializeField] protected LayerMask groundLayer;

    protected StatHandler statHandler;

    protected int damgage;
    protected float speed;

    protected ProjectileHandler projectileHandler;

    protected float currentDuration;
    protected Vector2 direction;
    protected bool isReady;
    protected Transform pivot;

    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer spriteRenderer;

    protected ProjectileManager projectileManager;

    protected Action<GameObject> returnToPool;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    protected virtual void Update()
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

        _rigidbody.velocity = direction * projectileHandler.Speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D hit)
    {
        if (groundLayer.value == (groundLayer.value | (1 << hit.gameObject.layer)))
        {
            DestroyProjectile();
        }
        else if (hitLayer.value == (hitLayer.value | (1 << hit.gameObject.layer)))
        {
            IDamagable target = hit.GetComponent<IDamagable>();
            if (target != null)
            {
                int damage = projectileHandler.Power;

                target.GetDamage(damage);
                Debug.Log($"{hit.name} 에게 {damage} 데미지를 입힘");
            }

            DestroyProjectile();
        }
    }
    
    public virtual void Init(Vector2 direction, ProjectileHandler inpuProjectileHandler, ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;

        projectileHandler = inpuProjectileHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * inpuProjectileHandler.BulletSize;
        spriteRenderer.color = inpuProjectileHandler.ProjectileColor;

        transform.right = this.direction;

        if (this.direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    protected virtual void DestroyProjectile()
    {
        // Destroy(this.gameObject);
        OnDespawn();
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }
}
