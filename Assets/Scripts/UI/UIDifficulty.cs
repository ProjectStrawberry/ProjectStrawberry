using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDifficulty : UIBase
{

    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button backButton;

    bool isAlreadyStarted = false;
    private void Start()
    {
        normalButton.onClick.AddListener(PressNormalButton);
        hardButton.onClick.AddListener(PressHardButton);
        backButton.onClick.AddListener(PressBackButton);

    }
    void PressNormalButton()
    {
        GameManager.Instance.ChangeDifficulty(Difficulty.Normal);
        SoundManager.Instance.ChangeBackGroundMusice(SoundManager.Instance.stageBgm);
        if (!isAlreadyStarted)
        {
            
            GameManager.Instance.StartGame();
            isAlreadyStarted = true;
            
        }
        else
        {
            GameManager.Instance.ReStart();
           
        }
        CloseUI();

    }

    void PressHardButton()
    {
        GameManager.Instance.ChangeDifficulty(Difficulty.Hard);
        SoundManager.Instance.ChangeBackGroundMusice(SoundManager.Instance.stageBgm);
        if (!isAlreadyStarted)
        {
            GameManager.Instance.StartGame();
            isAlreadyStarted = true;
        }
        else
        {
            GameManager.Instance.ReStart();
        }
        CloseUI();


    }
    


    void PressBackButton()
    {
        UIManager.Instance.OpenUI<UIStartScene>();
        CloseUI();
    }
}
