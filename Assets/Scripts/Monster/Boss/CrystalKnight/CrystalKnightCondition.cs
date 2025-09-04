using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightCondition : MonoBehaviour, IDamagable
{
    public CrystalKnight CrystalKnight;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] public float invincibleTime = 0.5f;

    private bool _isInvincible = false;
    private Coroutine _invincibleCoroutine;
    public event Action onTakeDamage;

    private void Awake()
    {
        CrystalKnight = GetComponent<CrystalKnight>();
        maxHealth = CrystalKnight.StatData.health;
        currentHealth = maxHealth;
    }

    public void GetDamage(int damage)
    {
        if (!_isInvincible)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Dead();
            }
            
            Debug.Log(CrystalKnight.name + "가 데미지 " + damage + "을 받음!");
            onTakeDamage?.Invoke();

            if (_invincibleCoroutine == null)
            {
                _invincibleCoroutine = StartCoroutine(InvincibleCoroutine());
            }
            else
            {
                Debug.LogError(_invincibleCoroutine + "이 null이 아닙니다!");
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        _isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        _isInvincible = false;
        _invincibleCoroutine = null;
    }

    public void Dead()
    {
        CrystalKnight.StateMachine.ChangeState(CrystalKnight.StateMachine.DeathState);
        // UIManager의 클리어 메서드
    }
}
