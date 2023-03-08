using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private Collider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            P_Control player = collision.transform.GetComponent<P_Control>();
            if (player != null)
                player.Dead();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
}
