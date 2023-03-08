using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintFx : MonoBehaviour
{
    [SerializeField]
    private P_Control player;
    private ParticleSystem fx;
    private ParticleSystem.MainModule fx_main;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GetComponentInParent<P_Control>();
        if (fx == null)
        {
            fx = GetComponent<ParticleSystem>();
            fx_main = fx.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || fx == null)
            return;

        if (player.IsSprint)
        {
            float direction = 0;
            if (!player.IsWatchingRight)
                direction = 180;
            fx_main.startRotationY = direction * Mathf.Deg2Rad;
            if (!fx.isPlaying)
                fx.Play();
        }
        else
            fx.Stop();
    }
}
