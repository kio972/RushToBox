using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBox : MonoBehaviour
{
    [SerializeField]
    private P_Control player;
    [SerializeField]
    private Collider2D p_BodyCollider;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.IsJump)
            return;

        if (collision.transform.tag == "Platform")
        {
            isActive = true;
            p_BodyCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActive)
            return;

        if (collision.transform.tag == "Platform")
        {
            StartCoroutine(UtillHelper.ReActiveCollider(p_BodyCollider, 0.05f));
            //p_BodyCollider.isTrigger = false;
            isActive = false;
        }
    }
}
