using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Collider2D collider;

    private void GetCoin()
    {
        GameManager.Instance.CollectCoin();
        this.gameObject.SetActive(false);
        if (AudioManager.Instance.audioClipController == null)
            return;
        AudioManager.Instance.Play2DSound(AudioManager.Instance.audioClipController.p_collectCoin, SettingUI.Instance.SoundFxVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            P_Control player = collision.transform.GetComponent<P_Control>();
            if (player != null)
                GetCoin();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
}
