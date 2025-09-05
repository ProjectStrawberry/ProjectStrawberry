using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIBase
{

    public Slider bgmSlider;
    public Slider sfxSlider;
    [SerializeField] Button backButton;
    void Start()
    {
        backButton.onClick.AddListener(PressBackButton);
        SoundManager.Instance.GetSettingsUI(this);
    }

    // Update is called once per frame
    void PressBackButton()
    {
        this.gameObject.SetActive(false);
        ButtonSound();
    }
    
}
