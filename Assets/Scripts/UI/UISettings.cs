using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIBase
{

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button backButton;
    void Start()
    {
        backButton.onClick.AddListener(PressBackButton);
    }

    // Update is called once per frame
    void PressBackButton()
    {
        this.gameObject.SetActive(false);
    }
}
