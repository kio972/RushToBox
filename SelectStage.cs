using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    private StageBtn[] stageBtns;
    // Start is called before the first frame update
    void Start()
    {
        stageBtns = GetComponentsInChildren<StageBtn>();
        foreach(StageBtn btn in stageBtns)
        {
            int stage = int.Parse(btn.gameObject.name);
            btn.Init(stage);
        }
    }
}
