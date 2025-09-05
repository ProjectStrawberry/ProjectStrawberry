using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderLaser : MonoBehaviour
{
    [SerializeField] private AudioClip thunderSFX;
    [SerializeField] private int damage;
    private bool isDamaged = false;

    private void Start()
    {
        SoundManager.PlayClip(thunderSFX, true);
    }

    public void Init(int damage)
    {
        this.damage = damage;
        isDamaged = false;
    }
    
    public void EndThunderLaser()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(this.name + "가 플레이어에게 데미지");
            if (other.TryGetComponent(out IDamagable playerCondition))
            {
                if (!isDamaged)
                {
                    Debug.Log(this.name + "가 플레이어를 찾았습니다");
                    playerCondition.GetDamage(damage);
                    isDamaged = true;
                }
            }
        }
        else
        {
            return;
        }
    }
}
