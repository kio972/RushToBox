using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageInfo : MonoBehaviour
{
    [SerializeField]
    private Button closeZone;
    [SerializeField]
    private Transform dialog;
    [SerializeField]
    private TextMeshProUGUI subTitle;
    [SerializeField]
    private TextMeshProUGUI limitTime;
    [SerializeField]
    private TextMeshProUGUI coinNumber;
    [SerializeField]
    private Button startBtn;
    private string sceneName;
    [SerializeField]
    StagePlayerMovement playerMover;

    private bool isMoving = false;

    private float iSizeW = 1920;
    private float iSizeH = 1080;

    private void StartStage()
    {
        if (isMoving)
            return;
        isMoving = true;
        playerMover.PlayAttackAnim();
        SceneController.Instance.MoveScene(sceneName);
    }

    private void CloseInfo()
    {
        SetActive(false);
    }

    private void SetDialogPos(Vector3 position)
    {
        Vector3 nextPosition = Camera.main.WorldToScreenPoint(position);
        float yDist = 370;
        if (nextPosition.y > (iSizeH / 2))
            nextPosition.y -= yDist;
        else
            nextPosition.y += yDist;

        float xDist = 650;
        if (nextPosition.x < xDist)
            nextPosition.x = xDist;
        else if (nextPosition.x > (iSizeW - xDist))
            nextPosition.x = (iSizeW - xDist);
        dialog.transform.position = nextPosition;
    }

    public void SetStageInfo(int stageNumber, Vector3 position)
    {
        subTitle.text = "Stage " + stageNumber.ToString() + " : " + DataHelper.Instance.GetStageSubTitle(stageNumber);
        limitTime.text = "Limit Time : " + (DataHelper.Instance.GetStageLimitTime(stageNumber)).ToString() + ".00";
        coinNumber.text = "Coin Number : " + (DataHelper.Instance.GetStageMaxCoin(stageNumber)).ToString();
        sceneName = "Stage" + stageNumber.ToString();
        SetDialogPos(position);
        playerMover.SetTargetPos(stageNumber);
        SetActive(true);
    }

    private void SetActive(bool value)
    {
        dialog.gameObject.SetActive(value);
        closeZone.gameObject.SetActive(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(StartStage);
        closeZone.onClick.AddListener(CloseInfo);
        SetActive(false);
        Canvas canvas = GetComponentInParent<Canvas>();
        Vector2 canvasSize = canvas.renderingDisplaySize;
        iSizeW = canvasSize.x;
        iSizeH = canvasSize.y;
    }

}
