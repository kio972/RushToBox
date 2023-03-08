using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerMovement : MonoBehaviour
{
    public P_Control player;
    public Transform firstPos;
    public Transform secendPos;
    public Transform thirdPos;
    public Transform fourthPos;
    public Transform resetPos;
    public float waitTime = 5;
    
    private IEnumerator ITitlePlayerMovement()
    {
        while(true)
        {
            float elapsed = 0f;
            while(elapsed < waitTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            player.inputAxis = new Vector2(-1, 0);
            while (Mathf.Abs(player.transform.position.x - firstPos.position.x) > 0.1f)
                yield return null;
            player.inputAxis = new Vector2(0, 0);

            elapsed = 0f;
            while (elapsed < waitTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            player.inputAxis = new Vector2(1, 0);
            while (Mathf.Abs(player.transform.position.x - secendPos.position.x) > 0.1f)
                yield return null;
            player.inputAxis = new Vector2(0, 0);

            player.SetClimb(true);
            while (Mathf.Abs(player.transform.position.y - thirdPos.position.y) > 0.1f)
                yield return null;
            player.SetClimb(false);
            elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            player.DoAttack();

            elapsed = 0f;
            while (elapsed < waitTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            player.inputAxis = new Vector2(1, 0);
            player.SetSprint(true);
            while (Mathf.Abs(player.transform.position.x - fourthPos.position.x) > 0.1f)
                yield return null;
            player.inputAxis = new Vector2(0, 0);
            player.SetSprint(false);
            player.transform.position = resetPos.position;
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(ITitlePlayerMovement());
    }
}
