using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePlayerMovement : MonoBehaviour
{
    private Vector3 targetPos;
    private P_Control player;
    [SerializeField]
    private Transform boxPosGroup;

    private IEnumerator IPlayerMove()
    {
        while(true)
        {
            if(Mathf.Abs((targetPos - player.transform.position).magnitude) > 0.1f)
            {
                Vector2 playerAxis = Vector2.zero;
                if (targetPos.x > player.transform.position.x)
                    playerAxis = new Vector2(1, 0);
                else
                    playerAxis = new Vector2(-1, 0);
                
                player.inputAxis = playerAxis;
                while (Mathf.Abs((targetPos.x - player.transform.position.x)) > 0.1f)
                    yield return null;

                player.inputAxis = Vector2.zero;
                bool needJump = false;
                if (targetPos.y > player.transform.position.y && Mathf.Abs(targetPos.y - player.transform.position.y) > 1.5f)
                {
                    needJump = true;
                    playerAxis = Vector2.up;
                }
                else if (targetPos.y < player.transform.position.y && Mathf.Abs(targetPos.y - player.transform.position.y) > 1.5f)
                {
                    needJump = true;
                    playerAxis = Vector2.down;
                }

                player.inputAxis = Vector2.zero;
                yield return null;
                player.RotateDirection(true);

                if (needJump)
                {
                    player.inputAxis = playerAxis;
                    player.DoJump();
                    float elapsed = 0f;
                    while(elapsed < 1f)
                    {
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                    player.inputAxis = Vector2.zero;
                }
            }

            yield return null;
        }
    }

    public void PlayAttackAnim()
    {
        if (Mathf.Abs((targetPos - player.transform.position).magnitude) > 0.1f)
            return;

        player.DoAttack();
    }

    public void SetTargetPos(int stageNumber)
    {
        ClearBox box = UtillHelper.Find<ClearBox>(boxPosGroup, "ClearBox" + stageNumber.ToString());
        if (box == null)
            return;

        targetPos = box.transform.position + new Vector3(-0.75f, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<P_Control>();
        targetPos = player.transform.position;
        StartCoroutine(IPlayerMove());
    }
}
