using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;
    }
}
