using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClear :UIBase
{

    [SerializeField] Button restartButton;
    [SerializeField] Button mainmenuButton;
    void PressRestartButton()
    {
        ButtonSound();
        //����� �������� ����
    }

    public void PressMainMenuButton()
    {
        ButtonSound();
        UIManager.Instance.CloseUI<UIClear>();
        UIManager.Instance.OpenUI<UIStartScene>();
    }

}
