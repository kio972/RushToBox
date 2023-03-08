using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionBtn : MonoBehaviour
{
    [SerializeField]
    private Button btn;
    private void CallSetting()
    {
        SettingUI.Instance.SetActive(true, GameManager.Instance.IsIngame);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (btn == null)
            btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(CallSetting);
    }
}
