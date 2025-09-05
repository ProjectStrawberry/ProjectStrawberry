using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : UIBase
{

    [SerializeField] Button restartButton;
    [SerializeField] Button mainmenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(PressRestartButton);
        mainmenuButton.onClick.AddListener(PressMainMenuButton);
    }
    void PressRestartButton()
    {
        GameManager.Instance.ReStart();
        CloseUI();
        ButtonSound();
    }

    void PressMainMenuButton()
    {
        UIManager.Instance.OpenUI<UIStartScene>();
        CloseUI();
        ButtonSound();
    }
}

