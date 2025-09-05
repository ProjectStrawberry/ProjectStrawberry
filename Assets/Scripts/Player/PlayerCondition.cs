using System;
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

    private Action<int> OnHealthChange;
    private Action<int> OnStaminaChange;

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
        _currHealth=_maxHealth;
        _currStemina=_maxStemina;
    }

    public void Heal(int amount)
    {
        _currHealth = Math.Min(_maxHealth, _currHealth + amount);
        OnHealthChange?.Invoke(_currHealth);
    }

    public void RestoreStemina()
    {
        _currStemina = Math.Min(_maxStemina, _currStemina + 1);
        OnStaminaChange?.Invoke(_currStemina);
    }

    public bool UseStemina()
    {
        if (_currStemina <= 0) return false;

        _currStemina -= 1;
        OnStaminaChange?.Invoke(_currStemina);
        return true;
    }


    public void Dead()
    {
        GameManager.Instance.GameOver();
    }

    public void GetDamage(int damage)
    {
        _currHealth -= 1;

        StartCoroutine(playerController.Damaged());
        StartCoroutine(playerController.Invincible());
        OnHealthChange?.Invoke(_currHealth);
        Debug.Log("플레이어 현재 체력: " + _currHealth);
        if( _currHealth <= 0 ) Dead();
    }

    public void SubscribeOnHealthChange(Action<int> action)
    {
        OnHealthChange += action;
    }


    public void SubscribeOnStaminaChange(Action<int> action)
    {
        OnStaminaChange += action;
    }

    public void ResetHealthAndStamina()
    {
        _maxHealth = (int)statHandler.GetStat(StatType.Health);
        _maxStemina = (int)statHandler.GetStat(StatType.Stemina);
        _currHealth = _maxHealth;
        _currStemina = _maxStemina;
    }
}
