using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIStartScene : UIBase
{
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
        ButtonSound();
    }

    void PressOptionsButton()
    {
        UIManager.Instance.OpenUI<UISettings>();
        ButtonSound();
    }

    void PressExitButton()
    {
        ButtonSound();

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
