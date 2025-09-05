using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackColliderHandler : MonoBehaviour
{
    public Skeleton Skeleton;
    public Collider2D attackCollider;

    private void Awake()
    {
        Skeleton = GetComponentInParent<Skeleton>();
        attackCollider = GetComponent<BoxCollider2D>();
        attackCollider.enabled = false;
    }

    public void TurnOnAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void TurnOffAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other: " + other);
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 데미지");
            if (other.TryGetComponent(out IDamagable playerCondition))
            {
                Debug.Log("플레이어를 찾았습니다");
                playerCondition.GetDamage(Skeleton.StatData.damage);
            }
        }
        else
        {
            return;
        }
    }
}
