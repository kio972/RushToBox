using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgmClip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBackground(bgmClip, SettingUI.Instance.MusicFxVolume);
    }
}
