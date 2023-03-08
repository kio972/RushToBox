using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoveSceneBtn moveSceneBtn = GetComponent<MoveSceneBtn>();
        
        int nextStage = SaveManager.Instance.data.stageProgress.Length;
        for(int i = 0; i < SaveManager.Instance.data.stageProgress.Length; i++)
        {
            if(SaveManager.Instance.data.stageProgress[i] == 0)
            {
                nextStage = i + 1;
                break;
            }
        }

        string nextSceneName = "Stage" + nextStage.ToString();
        moveSceneBtn.sceneName = nextSceneName;
    }
}
