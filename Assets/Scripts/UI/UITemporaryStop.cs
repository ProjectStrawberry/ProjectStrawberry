using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITemporaryStop : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button backToMainButton;

    void Start()
    {
        resumeButton.onClick.AddListener(PressResumeButton);
        optionsButton.onClick.AddListener(PressOptionsButton);
        backToMainButton.onClick.AddListener(PressBackToMainButton);
    }
    
    void PressResumeButton()
    {
        //���� �ٽ� resume
    }

    void PressOptionsButton()
    {
        UIManager.Instance.OpenUI<UISettings>();
    }

    void PressBackToMainButton()
    {
        UIManager.Instance.OpenUI<UIStartScene>();
    }
}
