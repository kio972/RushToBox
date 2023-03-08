using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour
{
    public List<Transform> boxMovePosList;
    private ClearBox box;
    private int curBoxHealth;
    public float delayTime = 0.1f;

    private void MoveBox()
    {
        box.transform.position = boxMovePosList[0].position;
        boxMovePosList.RemoveAt(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (box == null)
            box = GetComponent<ClearBox>();
        if(box != null)
            curBoxHealth = box.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (box == null)
            return;
        if (boxMovePosList.Count == 0)
            return;

        if(curBoxHealth != box.health)
        {
            curBoxHealth = box.health;
            StartCoroutine(UtillHelper.DelayedFunctionCall(MoveBox, delayTime));
        }
    }
}
