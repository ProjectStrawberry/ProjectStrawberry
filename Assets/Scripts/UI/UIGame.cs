using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : UIBase
{
    [SerializeField] RectTransform healthbarRect;
    [SerializeField] RectTransform staminabarRect;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject staminaBar;
    [SerializeField] GameObject healthIcon;
    [SerializeField] GameObject staminaIcon;

    PlayerCondition playerCondition;
    List<GameObject> healthList;
    List<GameObject> staminaList;
    Difficulty currentdifficulty;

    int maxHealth;
    int maxStamina;
    void Start()
    {
        GameManager.Instance.SubscribeOnDifficultyChange(ChangeUIDifficulty);
        playerCondition = PlayerManager.Instance.player.playerCondition;
        maxHealth = playerCondition._maxHealth;
        maxStamina=playerCondition._maxStemina;
        for(int i=0; i<maxHealth; i++)
        {
            Instantiate(healthIcon, healthbarRect);
        }
        for (int i = 0; i <maxStamina; i++)
        {
            Instantiate(staminaIcon,staminabarRect);
        }
        playerCondition.SubscribeOnHealthChange(ChangeHealthNumber);
        playerCondition.SubscribeOnStaminaChange(ChangeStaminaNumber);
        currentdifficulty=GameManager.Instance.currentDifficulty;
        ChangeUIDifficulty(currentdifficulty);
    }

    

    void ChangeUIDifficulty(Difficulty difficulty)
    {
        if(difficulty==Difficulty.Normal)
        {
            Vector2 healthRect = healthbarRect.sizeDelta;
            healthRect.x = 520;
            healthbarRect.sizeDelta = healthRect;

            Vector2 staminaRect = staminabarRect.sizeDelta;
            staminaRect.x = 240;
            staminabarRect.sizeDelta=staminaRect;
            maxHealth = playerCondition._maxHealth;
            maxStamina = playerCondition._maxStemina;
        }
        else
        {
            Vector2 healthRect = healthbarRect.sizeDelta;
            healthRect.x = 320;
            healthbarRect.sizeDelta = healthRect;

            Vector2 staminaRect = staminabarRect.sizeDelta;
            staminaRect.x =90;
            staminabarRect.sizeDelta = staminaRect;
            maxHealth = playerCondition._maxHealth;
            maxStamina = playerCondition._maxStemina;
        }
    }

    void ChangeHealthNumber(int currentHealth)
    {
        //현재체력
        for(int i = 0;i < currentHealth;i++)
        {
            healthList[i].gameObject.SetActive(true);
        }
    }

    void ChangeStaminaNumber(int currentStamina)
    {
        for (int i = 0; i < currentStamina; i++)
        {
            staminaList[i].gameObject.SetActive(true);
        }
    }
}
