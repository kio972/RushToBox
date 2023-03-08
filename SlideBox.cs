using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBox : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D p_RigidBody;
    [SerializeField]
    private P_Control player;

    private float prevGravityScale;
    public float slideGravityScale = 0.2f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "WallSlide")
        {
            if (player.IsJump)
                return;
            if(player.IsGround)
            {
                if(player.isWallSlide)
                    OnTriggerExit2D(collision);
                return;
            }

            if(!player.isWallSlide)
            {
                player.isWallSlide = true;
                //player.isWalljump = false;
                p_RigidBody.velocity = Vector2.zero;
                p_RigidBody.gravityScale = slideGravityScale;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "WallSlide")
        {
            p_RigidBody.gravityScale = prevGravityScale;
            player.isWallSlide = false;
        }
    }

    private void Start()
    {
        prevGravityScale = p_RigidBody.gravityScale;
    }
}
