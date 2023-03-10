using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource background;
    private List<AudioSource> effectAudioList = new List<AudioSource>();
    private Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
    // 오브젝트 풀링의 제한 개수
    private int limitCount = 5;
    // 제한 개수 이상이 되었을 경우 파괴시킬 시간 간격
    private float intervalTime = 1.0f;
    private float prevTime = 0;
    public AudioClipController audioClipController;


    public override void Init()
    {
        background = gameObject.AddComponent<AudioSource>();

        background.spatialBlend = 0;
        background.volume = 1.0f;

        background.playOnAwake = false;
        audioClipController = FindObjectOfType<AudioClipController>();
    }

    public void UpdateMusicVolume(float volume)
    {
        background.volume = volume;
    }

    public void UpdateFxVolume(float volume)
    {
        foreach(AudioSource effect in effectAudioList)
        {
            effect.volume = volume;
        }
    }

    public void PlayBackground(string name, float voulme = 1.0f)
    {
        if (clipDic.ContainsKey(name))
        {
            background.clip = clipDic[name];
            background.volume = voulme;
            background.Play();
        }
    }

    public void PlayBackground(AudioClip clip, float voulme = 1.0f)
    {
        if (clip != null)
        {
            background.clip = clip;
            background.volume = voulme;
            background.Play();
        }
    }

    // 오브젝트 풀링으로 오디오소스 관리
    AudioSource Pooling()
    {
        AudioSource audioSource = null;
        for (int i = 0; i < effectAudioList.Count; ++i)
        {
            if (effectAudioList[i].gameObject.activeSelf == false)
            {
                audioSource = effectAudioList[i];
                audioSource.gameObject.SetActive(true);
                break;
            }
        }
        if (audioSource == null)
        {
            audioSource = UtillHelper.CreateObject<AudioSource>(transform);
            effectAudioList.Add(audioSource);
        }
        return audioSource;
    }

    IEnumerator IDeactiveAudio(AudioSource audio)
    {
        yield return new WaitForSeconds(audio.clip.length);
        audio.gameObject.SetActive(false);
    }

    public void Play(string name, float spatialBelnd, float volume, Vector3 position)
    {
        if (clipDic.ContainsKey(name) == false)
            return;

        AudioSource audioSource = Pooling();
        audioSource.clip = clipDic[name];
        audioSource.spatialBlend = 0;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(IDeactiveAudio(audioSource));
    }

    public void Play(AudioClip clip, float spatialBelnd, float volume, Vector3 position)
    {
        if (clip == null)
            return;

        AudioSource audioSource = Pooling();
        audioSource.clip = clip;
        audioSource.spatialBlend = 0;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(IDeactiveAudio(audioSource));
    }

    public void Play2DSound(string name, float volume = 1.0f)
    {
        Play(name, 0, volume, Vector3.zero);
    }

    public void Play2DSound(AudioClip clip, float volume = 1.0f)
    {
        Play(clip, 0, volume, Vector3.zero);
    }

    public void Play3DSound(string name, Vector3 position, float volume = 1.0f)
    {
        Play(name, 1, volume, position);
    }

    // Update is called once per frame
    void Update()
    {
        if (effectAudioList.Count > limitCount)
        {
            float elapsedTime = Time.time - prevTime;
            // 경과 시간이 기준시간을 지났다면
            if (elapsedTime > intervalTime)
            {
                for (int i = 0; i < effectAudioList.Count; ++i)
                {
                    if (effectAudioList[i].gameObject.activeSelf == false)
                    {
                        AudioSource audioSource = effectAudioList[i];
                        effectAudioList.RemoveAt(i);
                        prevTime = Time.time;
                        Destroy(audioSource.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
