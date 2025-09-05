using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIBase
{
    [SerializeField] RectTransform healthbarRect;
    [SerializeField] RectTransform staminabarRect;
    [SerializeField] GameObject healthIcon;
    [SerializeField] GameObject staminaIcon;
    [SerializeField] Button stopButton;

    PlayerCondition playerCondition;
    List<GameObject> healthList= new List<GameObject>();
    List<GameObject> staminaList= new List<GameObject>();
    Difficulty currentdifficulty;

    int maxHealth;
    int maxStamina;
    void Start()
    {
        GameManager.Instance.SubscribeOnDifficultyChange(ChangeUIDifficulty);

        playerCondition = PlayerManager.Instance.player.playerCondition;
        maxHealth = playerCondition._maxHealth;
        Debug.Log(playerCondition._maxHealth);
        maxStamina=playerCondition._maxStemina;
        for(int i=0; i<maxHealth; i++)
        {
            
            GameObject go= Instantiate(healthIcon, healthbarRect,false);
            healthList.Add(go);
            
        }
        for (int i = 0; i <maxStamina; i++)
        {
            GameObject go= Instantiate(staminaIcon,staminabarRect,false);
            staminaList.Add(go);
        }
        playerCondition.SubscribeOnHealthChange(ChangeHealthNumber);
        playerCondition.SubscribeOnStaminaChange(ChangeStaminaNumber);
        currentdifficulty=GameManager.Instance.currentDifficulty;
        ChangeUIDifficulty(currentdifficulty);
        stopButton.onClick.AddListener(PressStopButton);

       


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
            ChangeHealthNumber(playerCondition._currHealth);
            ChangeStaminaNumber(playerCondition._currStemina);
        }
        else
        {
            Vector2 healthRect = healthbarRect.sizeDelta;
            healthRect.x = 320;
            healthbarRect.sizeDelta = healthRect;

            Vector2 staminaRect = staminabarRect.sizeDelta;
            staminaRect.x = 240;
            staminabarRect.sizeDelta = staminaRect;
            maxHealth = playerCondition._maxHealth;
            maxStamina = playerCondition._maxStemina;
            ChangeHealthNumber(playerCondition._currHealth);
            Debug.Log(playerCondition._currHealth);
            ChangeStaminaNumber(playerCondition._currStemina);
        }
    }
    public void ResetUI()
    {
        ChangeHealthNumber(PlayerManager.Instance.player.playerCondition._currHealth);
        ChangeStaminaNumber(PlayerManager.Instance.player.playerCondition._currStemina);
    }


    void ChangeHealthNumber(int currentHealth)
    {
        //현재체력
        Debug.Log(currentHealth);
        foreach(GameObject go in healthList)
        {
            go.SetActive(false);
        }
        for(int i = 0;i < currentHealth;i++)
        {
            Debug.Log(i);
            healthList[i].SetActive(true);
        }
        
    }

    void ChangeStaminaNumber(int currentStamina)
    {
        foreach (GameObject go in staminaList)
        {
            go.SetActive(false);
        }
        for (int i = 0; i < currentStamina; i++)
        {
            staminaList[i].SetActive(true);
        }
    }

    void PressStopButton()
    {
        UIManager.Instance.OpenUI<UITemporaryStop>();

    }
}
