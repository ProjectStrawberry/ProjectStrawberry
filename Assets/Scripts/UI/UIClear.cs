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

    void PressMainMenuButton()
    {
        ButtonSound();

    }

}
