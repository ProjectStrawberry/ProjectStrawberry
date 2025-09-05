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
        //저장된 지점으로 가기
    }

    void PressMainMenuButton()
    {
        ButtonSound();

    }

}
