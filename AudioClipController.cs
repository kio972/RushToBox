using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioClip mainMusic;
    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip stage3Music;
    public AudioClip stage4Music;
    public AudioClip stage5Music;
    public AudioClip stage6Music;
    public AudioClip stage7Music;
    public AudioClip stage8Music;
    public AudioClip stage9Music;
    public AudioClip stage10Music;
    public AudioClip p_attack;
    public AudioClip p_jump;
    public AudioClip p_left_foot;
    public AudioClip p_right_foot;
    public AudioClip p_wallSlide;
    public AudioClip p_collectCoin;
    public AudioClip p_death;
    public AudioClip box_hit;
    public AudioClip box_destroyed;

    private void Awake()
    {
        AudioClipController[] temp = FindObjectsOfType<AudioClipController>();
        if (temp.Length > 1)
            Destroy(this.gameObject);
        else
        {
            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(this.transform.root.gameObject);
            else
                DontDestroyOnLoad(this.gameObject);
        }

        print(mainMusic);
    }
}
