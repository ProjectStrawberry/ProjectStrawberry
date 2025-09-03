using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIBase
{

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void PressBackButton()
    {
        this.gameObject.SetActive(false);
    }
}
