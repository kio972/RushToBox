using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBox : MonoBehaviour
{
    public int health = 1;
    [SerializeField]
    private Animator animator;

    private bool isInvincible = false;
    private float invincibleTime = 1f;

    private IEnumerator InvincibleTimer()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        while(elapsedTime < invincibleTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isInvincible = false;
    }

    private void DestroyBox()
    {
        animator.SetBool("Destroy", true);
        WinStage();
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.box_destroyed, SettingUI.Instance.SoundFxVolume);
    }

    public void GetDamage(int damage = 1)
    {
        if (isInvincible)
            return;

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            DestroyBox();
        }
        else
        {
            animator.SetTrigger("Damaged");
            if (AudioManager.Instance.audioClipController == null)
                return;
            AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.box_hit, SettingUI.Instance.SoundFxVolume);
        }

        StartCoroutine(InvincibleTimer());
    }

    public void WinStage()
    {
        GameManager.Instance.EndStage(true);
    }

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }
}
