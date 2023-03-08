using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageBtn : MonoBehaviour
{
    public int stageNumber = 0;
    private Button moveBtn;
    private Image locker;
    private Image star1;
    private Image star2;
    private Image star3;
    [SerializeField]
    private bool isLocked = false;
    private TextMeshProUGUI stageText;
    
    private void CallInfo()
    {
        if (isLocked)
            return;

        StageInfo stageInfo = FindObjectOfType<StageInfo>();
        if(stageInfo != null)
            stageInfo.SetStageInfo(stageNumber, transform.position);
    }

    private void SetStar()
    {
        int starCount = SaveManager.Instance.data.stageProgress[stageNumber - 1];
        bool star1 = false;
        bool star2 = false;
        bool star3 = false;
        switch(starCount)
        {
            case 1:
                star1 = true;
                break;
            case 2:
                star1 = true;
                star2 = true;
                break;
            case 3:
                star1 = true;
                star2 = true;
                star3 = true;
                break;
        }
        this.star1.gameObject.SetActive(star1);
        this.star2.gameObject.SetActive(star2);
        this.star3.gameObject.SetActive(star3);
    }

    private void SetLocker()
    {
        bool isLocked = true;
        int prevStage = stageNumber - 1;
        if (prevStage < 1)
            isLocked = false;
        else if(SaveManager.Instance.data.stageProgress[prevStage - 1] >= 1)
            isLocked = false;

        this.isLocked = isLocked;
        locker.gameObject.SetActive(isLocked);
    }

    public void Init(int stageNumber)
    {
        this.stageNumber = stageNumber;
        moveBtn = UtillHelper.Find<Button>(transform, "MoveButton");
        moveBtn.onClick.AddListener(CallInfo);
        stageText = UtillHelper.Find<TextMeshProUGUI>(moveBtn.transform, "StageNumber/Text");
        stageText.text = "Stage" + stageNumber.ToString();
        locker = UtillHelper.Find<Image>(moveBtn.transform, "Lock");
        star1 = UtillHelper.Find<Image>(transform, "Stars/Star1/On");
        star2 = UtillHelper.Find<Image>(transform, "Stars/Star2/On");
        star3 = UtillHelper.Find<Image>(transform, "Stars/Star3/On");
        SetStar();
        SetLocker();
    }
}
