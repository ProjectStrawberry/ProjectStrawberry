using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private LayerMask groundLayer;


    private StatHandler statHandler;

    private int damgage;
    private float speed;

    private ProjectileHandler projectileHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    private ProjectileManager projectileManager;

    private Action<GameObject> returnToPool;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
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

    private void OnTriggerEnter2D(Collider2D hit)
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


    public void Init(Vector2 direction, ProjectileHandler inpuProjectileHandler, ProjectileManager projectileManager)
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

    private void DestroyProjectile()
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
