using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingUI : Singleton<SettingUI>
{
    private bool isInit = false;

    private float soundFxVolume = 1f;
    private float musicVolume = 1f;

    private bool isOpen = false;

    private bool isSoundFxTrue;
    private bool isMusicFxTrue;

    [SerializeField]
    private Toggle soundFxToggle;
    [SerializeField]
    private Toggle musicFxToggle;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private TextMeshProUGUI sfxVolumePercent;
    [SerializeField]
    private TextMeshProUGUI musicVolumePercent;

    [SerializeField]
    private Transform dialog;
    [SerializeField]
    private Transform inGameButtons;
    [SerializeField]
    private Button stageSelectBtn;
    [SerializeField]
    private Button restartBtn;
    [SerializeField]
    private Button closeBtn;

    public float SoundFxVolume
    {
        get
        {
            if (!soundFxToggle.isOn)
                return 0;
            return soundFxVolume;
        }
    }

    public float MusicFxVolume
    {
        get
        {
            if (!musicFxToggle.isOn)
                return 0;
            return musicVolume;
        }
    }

    private void CloseSetting()
    {
        SetActive(false);
        
        if (GameManager.Instance.IsPause)
            GameManager.Instance.Resume();
    }

    private void UpdateValue()
    {
        bool isSaveNeed = false;
        if(isSoundFxTrue != soundFxToggle.isOn)
        {
            isSoundFxTrue = soundFxToggle.isOn;
            AudioManager.Instance.UpdateFxVolume(SoundFxVolume);
            isSaveNeed = true;
        }

        if (soundFxVolume != sfxVolumeSlider.value)
        {
            soundFxVolume = sfxVolumeSlider.value;
            int percent = (int)(soundFxVolume * 100);
            sfxVolumePercent.text = percent.ToString() + "%";
            AudioManager.Instance.UpdateFxVolume(SoundFxVolume);
            isSaveNeed = true;
        }

        if(isMusicFxTrue != musicFxToggle.isOn)
        {
            isMusicFxTrue = musicFxToggle.isOn;
            AudioManager.Instance.UpdateMusicVolume(MusicFxVolume);
            isSaveNeed = true;
        }

        if(musicVolume != musicVolumeSlider.value)
        {
            musicVolume = musicVolumeSlider.value;
            int percent = (int)(musicVolume * 100);
            musicVolumePercent.text = percent.ToString() + "%";
            AudioManager.Instance.UpdateMusicVolume(MusicFxVolume);
            isSaveNeed = true;
        }

        if (isSaveNeed)
            SaveSettingValue();
    }

    public void SetActive(bool value, bool isIngame = false)
    {
        dialog.gameObject.SetActive(value);
        inGameButtons.gameObject.SetActive(isIngame);
        isOpen = value;
    }

    private void StageSelect()
    {
        GameManager.Instance.Resume();
        CloseSetting();
        SceneController.Instance.MoveScene("StageSelect");
    }

    private void StageRestart()
    {
        GameManager.Instance.Resume();
        CloseSetting();
        SceneController.Instance.RestartScene();
    }

    private void SaveSettingValue()
    {
        SaveManager.Instance.data.soundFxToggle = soundFxToggle.isOn;
        SaveManager.Instance.data.musicFxToggle = musicFxToggle.isOn;
        SaveManager.Instance.data.soundFxSetting = sfxVolumeSlider.value;
        SaveManager.Instance.data.musicFxSetting = musicVolumeSlider.value;
        SaveManager.Instance.SaveGameData();
    }

    private void LoadSettingValue()
    {
        soundFxToggle.isOn = SaveManager.Instance.data.soundFxToggle;
        musicFxToggle.isOn = SaveManager.Instance.data.musicFxToggle;
        sfxVolumeSlider.value = SaveManager.Instance.data.soundFxSetting;
        musicVolumeSlider.value = SaveManager.Instance.data.musicFxSetting;
        UpdateValue();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!isInit)
        {
            SettingUI[] temp = FindObjectsOfType<SettingUI>();
            if (temp.Length > 1)
                Destroy(this.gameObject);
            else
            {
                stageSelectBtn.onClick.AddListener(StageSelect);
                restartBtn.onClick.AddListener(StageRestart);
                closeBtn.onClick.AddListener(CloseSetting);
                SetActive(false);
                isSoundFxTrue = soundFxToggle.isOn;
                isMusicFxTrue = soundFxToggle.isOn;
                LoadSettingValue();
                isInit = true;
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
            UpdateValue();
    }
}
