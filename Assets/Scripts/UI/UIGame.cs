using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : UIBase
{
    [SerializeField] RectTransform healthbarRect;
    [SerializeField] RectTransform staminabarRect;

    void Start()
    {
        GameManager.Instance.SubscribeOnDifficultyChange(ChangeUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeUI(Difficulty difficulty)
    {
        if(difficulty==Difficulty.Normal)
        {

        }
        else
        {

        }
    }
}
