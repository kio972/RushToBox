using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
    private bool isPlayerEnter = false;
    private Collider2D collider;
    public void PlayerExit()
    {
        if (!isPlayerEnter)
            return;

        Invoke("ActiveCollider", 0.1f);
    }

    private void ActiveCollider()
    {
        isPlayerEnter = false;
        collider.isTrigger = true;
    }

    public void PlayerEnter()
    {
        if (isPlayerEnter)
            return;

        isPlayerEnter = true;
        collider.isTrigger = false;
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }
}
