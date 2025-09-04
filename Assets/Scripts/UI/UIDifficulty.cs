using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDifficulty : UIBase
{

    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button backButton;
    private void Start()
    {
        normalButton.onClick.AddListener(PressNormalButton);
        hardButton.onClick.AddListener(PressHardButton);
        backButton.onClick.AddListener(PressBackButton);

    }
    void PressNormalButton()
    {
        GameManager.Instance.ChangeDifficulty(Difficulty.Normal);
        GameManager.Instance.StartGame();
        CloseUI();
    }

    void PressHardButton()
    {
        GameManager.Instance.ChangeDifficulty(Difficulty.Hard);
        GameManager.Instance.StartGame();
        CloseUI();
    }

    void PressBackButton()
    {
        UIManager.Instance.OpenUI<UIStartScene>();
        CloseUI();
    }
}
