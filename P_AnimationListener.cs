using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_AnimationListener : MonoBehaviour
{
    void JumpFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_jump, SettingUI.Instance.SoundFxVolume);
    }

    void AttackFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_attack, SettingUI.Instance.SoundFxVolume);
    }

    void WallSlideFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_wallSlide, SettingUI.Instance.SoundFxVolume);
    }

    void FootRFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_right_foot, SettingUI.Instance.SoundFxVolume);
    }

    void FootLFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_left_foot, SettingUI.Instance.SoundFxVolume);
    }

    void DeathFx()
    {
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_death, SettingUI.Instance.SoundFxVolume);
    }
}
