using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private Transform resultPage;
    [SerializeField]
    private Button restartBtn;
    [SerializeField]
    private Button nextBtn;
    [SerializeField]
    private Button menuBtn;
    [SerializeField]
    private Button pauseBtn;
    [SerializeField]
    private Button loseMenuBtn;
    [SerializeField]
    private TextMeshProUGUI collectedCoin;
    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private TextMeshProUGUI stageClearTime;
    [SerializeField]
    private TextMeshProUGUI stageCollectedCoin;
    [SerializeField]
    private TextMeshProUGUI stageClearHeader;
    [SerializeField]
    private TextMeshProUGUI stageFailHeader;

    private int curStage;

    private Vector3 basePos;
    [SerializeField]
    private Transform star1;
    [SerializeField]
    private Transform star2;
    [SerializeField]
    private Transform star3;

    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip loseClip;
    [SerializeField]
    private AudioClip starClip;

    private void UpdateCoinNumber()
    {
        collectedCoin.text = GameManager.Instance.CollectedCoin.ToString();
    }

    private void UpdateTimer()
    {
        float time = GameManager.Instance.Timer;
        time = Mathf.Floor(time * 100f) / 100f;
        string limitTimeText = "00.00";
        float limitTime = DataHelper.Instance.GetStageLimitTime(curStage);
        if (limitTime != 0)
            limitTimeText = limitTime.ToString() + ".00";
        string text = time.ToString() + " / " + limitTimeText;
        timer.text = text;
    }

    private void RestartStage()
    {
        GameManager.Instance.Resume();
        SceneController.Instance.RestartScene();
    }

    private void GoNextStage()
    {
        int nextStage = curStage + 1;
        if(DataHelper.Instance.StageData.Count < nextStage)
            GoSelectStage();
        else
        {
            string nextStageName = "Stage" + nextStage.ToString();
            SceneController.Instance.MoveScene(nextStageName);
        }
    }

    private void GoSelectStage()
    {
        SceneController.Instance.MoveScene("StageSelect");
    }

    private void ResumeGameCheck()
    {
        if (!GameManager.Instance.IsPause)
            pauseBtn.gameObject.SetActive(true);
    }

    private void PauseGame()
    {
        GameManager.Instance.Pause();
        SettingUI.Instance.SetActive(true, true);
        pauseBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
    }

    private void StageClearInit(bool isWin)
    {
        stageClearHeader.gameObject.SetActive(isWin);
        stageFailHeader.gameObject.SetActive(!isWin);
        nextBtn.gameObject.SetActive(isWin);
        menuBtn.gameObject.SetActive(isWin);
        pauseBtn.gameObject.SetActive(false);
        loseMenuBtn.gameObject.SetActive(!isWin);
        float limitTime = DataHelper.Instance.GetStageLimitTime(curStage);
        stageClearTime.text = " " + " / " + limitTime.ToString() + ".00";
        int maxCoin = DataHelper.Instance.GetStageMaxCoin(curStage);
        stageCollectedCoin.text = "  " + " / " + maxCoin.ToString();
        stageCollectedCoin.text = collectedCoin.text + " / " + maxCoin.ToString();
        star1.localScale = new Vector3(0, 0, 0);
        star2.localScale = new Vector3(0, 0, 0);
        star3.localScale = new Vector3(0, 0, 0);
    }



    IEnumerator IStageEnd(int star)
    {
        // 초기화
        bool isWin = true;
        if (star == 0)
            isWin = false;
        StageClearInit(isWin);

        // 포지션 이동
        resultPage.gameObject.SetActive(true);
        Vector3 startPos = basePos + new Vector3(0, 1000, 0);
        resultPage.transform.position = startPos;
        float elapsedTime = 0f;
        float lerpTime = 0.5f;
        while(elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 nextPos = Vector3.Lerp(startPos, basePos, elapsedTime / lerpTime);
            resultPage.transform.position = nextPos;
            yield return null;
        }
        resultPage.transform.position = basePos;
        if (isWin)
        {
            UtillHelper.ActiveTrigger(stageClearHeader.transform, "Active");
            AudioManager.Instance.Play2DSound(winClip, SettingUI.Instance.SoundFxVolume);
        }
        else
        {
            UtillHelper.ActiveTrigger(stageFailHeader.transform, "Active");
            AudioManager.Instance.Play2DSound(loseClip, SettingUI.Instance.SoundFxVolume);
        }

        elapsedTime = 0f;
        while(elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }    

        // 별 처리
        int nextStar = 1;
        Transform curStar = star1;
        if (isWin)
        {
            AudioManager.Instance.Play2DSound(starClip, SettingUI.Instance.SoundFxVolume);
            // localeScale 0 -> 1.2 -> 1
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 0, 1.2f, 0.5f));
            while (curStar.localScale != new Vector3(1.2f, 1.2f, 1.2f))
                yield return null;
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 1.2f, 1f, 0.2f));
            while (curStar.localScale != new Vector3(1, 1, 1))
                yield return null;
        }


        // limitTiem 체크
        elapsedTime = 0f;
        lerpTime = 1f;
        float clearTime = GameManager.Instance.Timer;
        clearTime = Mathf.Floor(clearTime * 100f) / 100f;
        float limitTime = DataHelper.Instance.GetStageLimitTime(curStage);
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float time = Mathf.Lerp(0f, clearTime, elapsedTime / lerpTime);
            time = Mathf.Floor(time * 100f) / 100f;
            stageClearTime.text = time.ToString() + " / " + limitTime.ToString() + ".00";
            if (time > limitTime)
                stageClearTime.color = Color.red;
            yield return null;
        }
        stageClearTime.text = clearTime.ToString() + " / " + limitTime.ToString() + ".00";

        if (clearTime <= limitTime && isWin)
        {
            AudioManager.Instance.Play2DSound(starClip, SaveManager.Instance.data.soundFxSetting);

            nextStar++;
            curStar = star2;
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 0, 1.2f, 0.5f));
            while (curStar.localScale != new Vector3(1.2f, 1.2f, 1.2f))
                yield return null;
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 1.2f, 1f, 0.2f));
            while (curStar.localScale != new Vector3(1, 1, 1))
                yield return null;
        }

        // coin 체크
        elapsedTime = 0f;
        lerpTime = 1f;
        int clearCoin = GameManager.Instance.CollectedCoin;
        int maxCoin = DataHelper.Instance.GetStageMaxCoin(curStage);
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            int coin = (int)Mathf.Lerp(0, clearCoin, elapsedTime / lerpTime);
            stageCollectedCoin.text = coin.ToString() + " / " + maxCoin.ToString();
            yield return null;
        }
        stageCollectedCoin.text = clearCoin.ToString() + " / " + maxCoin.ToString();
        if (clearCoin < maxCoin)
            stageCollectedCoin.color = Color.red;

        if (clearCoin >= maxCoin && isWin)
        {
            AudioManager.Instance.Play2DSound(starClip, SaveManager.Instance.data.soundFxSetting);

            nextStar++;
            if (nextStar == 2)
                curStar = star2;
            else if (nextStar == 3)
                curStar = star3;
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 0, 1.2f, 0.5f));
            while (curStar.localScale != new Vector3(1.2f, 1.2f, 1.2f))
                yield return null;
            StartCoroutine(UtillHelper.ScaleLerp(curStar, 1.2f, 1f, 0.2f));
            while (curStar.localScale != new Vector3(1, 1, 1))
                yield return null;
        }
    }

    public void StageEnd(int star)
    {
        StartCoroutine(IStageEnd(star));
    }

    // Start is called before the first frame update
    void Start()
    {
        restartBtn.onClick.AddListener(RestartStage);
        nextBtn.onClick.AddListener(GoNextStage);
        menuBtn.onClick.AddListener(GoSelectStage);
        pauseBtn.onClick.AddListener(PauseGame);
        curStage = GameManager.Instance.StageNumber;
        basePos = resultPage.transform.position;
        resultPage.gameObject.SetActive(false);
        GameManager.Instance.StartStage();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCoinNumber();
        UpdateTimer();
        ResumeGameCheck();
    }
}
