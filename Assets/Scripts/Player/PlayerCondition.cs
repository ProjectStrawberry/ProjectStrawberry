using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    protected StatHandler statHandler;
    protected AnimationHandler animationHandler;
    protected PlayerController playerController;

    public int _maxHealth;
    public int _currHealth;
    public int _maxStemina;
    public int _currStemina;

    protected void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();
        playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        _maxHealth = (int)statHandler.GetStat(StatType.Health);
        _maxStemina = (int)statHandler.GetStat(StatType.Stemina);
    }

    public void Heal(int amount)
    {
        _currStemina += amount;
    }

    public void RestoreStemina()
    {
        _currStemina += 1;
    }

    public bool UseStemina()
    {
        if (_currStemina <= 0) return false;

        _currStemina -= 1;
        return true;
    }


    public void Dead()
    {
        playerController.Dead();
    }

    public void GetDamage(int damage)
    {
        _currHealth -= 1;
        StartCoroutine(playerController.Damaged());
        StartCoroutine(playerController.Invincible());
        Debug.Log("플레이어 현재 체력: " + _currHealth);
        if( _currHealth <= 0 ) Dead();
    }
}
