using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel.Design;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField][Range(0f, 1f)] private static float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private static float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    //[SerializeField] private bool isBgmMuted;
    //[SerializeField] private static bool isSfxMuted;
    private Slider bgmSlider;
    private Slider sfxSlider;
    //[SerializeField] private Toggle bgmToggle;
    //[SerializeField] private Toggle sfxToggle;

    private const string _BGMSlide = "BGMSlide";
    private const string _SFXSlide = "SFXSlide";
    //private const string _BGMMute = "BGMMute";
    //private const string _SFXMute = "SFXMute";



    private AudioSource musicAudioSource;
    public AudioClip titleBgm;
    public AudioClip stageBgm;
    public AudioClip bossBgm;

    public AudioClip buttonClip;
    public AudioClip clearClip;

    public GameObject soundSourcePrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        musicAudioSource = GetComponent<AudioSource>();
        PrefCheck();
        //musicAudioSource.mute = isBgmMuted;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        //musicClip = Resources.Load("Sound/BGM/사운드 이름") as AudioClip; //배경음악
        soundSourcePrefab = Resources.Load("Prefabs/SoundSource") as GameObject;


    }

    public void PrefCheck()
    {
        if (PlayerPrefs.HasKey("bgmVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("bgmVolume");
        }
        else
        {
            musicVolume = 1.0f;
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            soundEffectVolume = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            soundEffectVolume = 1.0f;
        }

        //if (PlayerPrefs.HasKey("isBgmMuted"))
        //{
        //    isBgmMuted = PlayerPrefs.GetInt("isBgmMuted") == 1 ? true : false;
        //}
        //else
        //{
        //    isBgmMuted = false;
        //}

        //if (PlayerPrefs.HasKey("isSfxMuted"))
        //{
        //    isSfxMuted = PlayerPrefs.GetInt("isSfxMuted") == 1 ? true : false;
        //}
        //else
        //{
        //    isSfxMuted = false;
        //}
        //bgmToggle.isOn = isBgmMuted;
        //sfxToggle.isOn = isSfxMuted;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //bgmSlider = GameObject.Find(_BGMSlide).GetComponent<Slider>();
        //sfxSlider = GameObject.Find(_SFXSlide).GetComponent<Slider>();
        //bgmToggle = GameObject.Find(_BGMMute).GetComponent<Toggle>();
        //sfxToggle = GameObject.Find(_SFXMute).GetComponent<Toggle>();

        
        //bgmToggle.onValueChanged.AddListener(BgmToggleChanged);
        //sfxToggle.onValueChanged.AddListener(SfxToggleChanged);

        PrefCheck();
    }

    public void BgmSliderChanged(float changedData)
    {
        musicVolume = changedData;
        musicAudioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("bgmVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SfxSliderChanged(float changedData)
    {
        soundEffectVolume = changedData;
        PlayerPrefs.SetFloat("sfxVolume", soundEffectVolume);
        PlayerPrefs.Save();
    }

    //public void BgmToggleChanged(bool changedData)
    //{
    //    isBgmMuted = changedData;
    //    musicAudioSource.mute = isBgmMuted;
    //    PlayerPrefs.SetInt("isBgmMuted", changedData ? 1 : 0);
    //    PlayerPrefs.Save();
    //}

    //public void SfxToggleChanged(bool changedData)
    //{
    //    isSfxMuted = changedData;
    //    //sfxAudioSource.mute = isSfxMuted;
    //    PlayerPrefs.SetInt("isSfxMuted", changedData ? 1 : 0);
    //    PlayerPrefs.Save();
    //}

    private void Start()
    {
        ChangeBackGroundMusice(titleBgm);
    }

    public void ChangeBackGroundMusice(AudioClip clip)
    {
        if (clip == musicAudioSource.clip) return;
        
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = Instantiate(Instance.soundSourcePrefab);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, SoundManager.soundEffectVolume, SoundManager.soundEffectPitchVariance);
    }

    public static GameObject PlayClip(AudioClip clip, bool isTrue)
    {
        GameObject obj = Instantiate(Instance.soundSourcePrefab);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, SoundManager.soundEffectVolume, SoundManager.soundEffectPitchVariance);
        if (isTrue) return obj;
        else return null;
    }

    public void GetSettingsUI(UISettings settings)
    {
        this.bgmSlider = settings.bgmSlider;
        this.sfxSlider = settings.sfxSlider;
        this.bgmSlider.value = musicVolume;
        this.sfxSlider.value = soundEffectVolume;
        this.bgmSlider.onValueChanged.AddListener(BgmSliderChanged);
        this.sfxSlider.onValueChanged.AddListener(SfxSliderChanged);
    }
}