using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class UIPopUp : UIBase
{


    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    public Action OnpressYes;
    public Action OnpressNo;

    public void OpenPopUP( string message, Action yesAction, Action noAction)
    {
        this.gameObject.SetActive(true);
        this.message.SetText(message);

        OnpressYes += yesAction;
        OnpressNo += noAction;

        yesButton.onClick.AddListener(PressYesButton);
        noButton.onClick.AddListener(PressNoButton);

    }


    private void PressYesButton()
    {
        OnpressYes?.Invoke();
    }

    private void PressNoButton()
    {
        OnpressNo?.Invoke();
    }


   
}
