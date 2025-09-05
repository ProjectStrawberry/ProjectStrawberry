using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightAttackHitBoxHandler : MonoBehaviour
{
    private CrystalKnight crystalKnight;

    [Header("공격 히트박스")] 
    public BoxCollider2D rushAttackCollider;
    public BoxCollider2D comboAttackCollider;

    private void Awake()
    {
        crystalKnight = GetComponentInParent<CrystalKnight>();

        rushAttackCollider.enabled = false;
        comboAttackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamagable playerCondition))
            {
                playerCondition.GetDamage(crystalKnight.StatData.damage);
            }
        }
        else
        {
            return;
        }
    }
}
