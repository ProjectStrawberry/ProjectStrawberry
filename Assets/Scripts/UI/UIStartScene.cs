using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIStartScene : UIBase
{
    // Start is called before the first frame update
    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;

   
    string messageText = "Quit the Game?";
    void Start()
    {
        startButton.onClick.AddListener(PressStartButton);
        optionsButton.onClick.AddListener(PressOptionsButton);
        exitButton.onClick.AddListener(PressExitButton);
    }



    void PressStartButton()
    {
        UIManager.Instance.OpenUI<UIDifficulty>();
        CloseUI();
    }

    void PressOptionsButton()
    {
        UIManager.Instance.OpenUI<UISettings>();
    }

    void PressExitButton()
    {
        
        
            var popUp = UIManager.Instance.GetUI<UIPopUp>();
            popUp.OpenPopUP(messageText, ExitGame, ClosePopup);

 
        
        
    }

    //구체적 행동도 여기 써야하나?

    void ExitGame()
    {
        Application.Quit();
    }

    void ClosePopup()
    {
        UIManager.Instance.CloseUI<UIPopUp>();
    }
}
