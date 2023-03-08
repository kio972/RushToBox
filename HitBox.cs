using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private P_Control player;
    private Collider2D hitBox;

    public void AttackCheck()
    {
        float watchingPos = 1;
        if (!player.IsWatchingRight)
            watchingPos *= -1;
        Vector2 origin = (Vector2)transform.position + new Vector2(hitBox.offset.x * watchingPos, hitBox.offset.y);
        Vector2 size = new Vector2(hitBox.bounds.size.x, hitBox.bounds.size.y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(origin, size, 0, LayerMask.GetMask("Object"));
        foreach(Collider2D hit in hits)
        {
            if (hit.transform.tag == "Box")
            {
                ClearBox box = hit.transform.GetComponent<ClearBox>();
                box.GetDamage();
            }
        }
    }

    private void Start()
    {
        hitBox = GetComponent<Collider2D>();
    }

}
