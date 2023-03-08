using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isIngame = false;
    public bool IsIngame { get { return isIngame; } }
    // Start is called before the first frame update

    private float timer = 0;
    public float Timer { get { return timer; } }
    private int collectedCoin = 0;
    public int CollectedCoin { get { return collectedCoin; } }
    public string sceneName = null;

    Coroutine timerCoroutine = null;

    private bool isPause = false;
    public bool IsPause { get { return isPause; } }
    private int starNumber = 0;

    public int StageNumber
    {
        get
        {
            string stage = UtillHelper.GetCurrentSceneName();
            if (!stage.Contains("Stage"))
                return 0;
            stage = stage.Remove(0, 5);
            int stageNumber = 0;
            if (int.TryParse(stage, out stageNumber))
                return stageNumber;
            return 0;
        }
    }

    public void CollectCoin(int number = 1)
    {
        collectedCoin += number;
    }

    public void Pause()
    {
        if (isPause)
            return;

        isPause = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        if (!isPause)
            return;

        isPause = false;
        Time.timeScale = 1f;
    }

    IEnumerator ITimer()
    {
        timer = 0;
        while(true)
        {
            timer += Time.deltaTime;
            if (timer > 999.99f)
            {
                timer = 999.99f;
                break;
            }
            yield return null;
        }
    }

    public void StartTimer()
    {
        StopTimer();
        timerCoroutine = StartCoroutine(ITimer());
    }


    public void StopTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public int CalculateStar(bool isWin = true)
    {
        if (!isWin)
            return 0;
        int winStar = 1;
        float timeLimit = DataHelper.Instance.GetStageLimitTime(StageNumber);
        if (timer <= timeLimit || timeLimit == 0)
            winStar += 1;
        int maxCoin = DataHelper.Instance.GetStageMaxCoin(StageNumber);
        if (collectedCoin >= maxCoin)
            winStar += 1;

        return winStar;
    }

    public void EndStage(bool isWin)
    {
        StopTimer();
        isPause = true;
        starNumber = CalculateStar(isWin);
        GameData data = SaveManager.Instance.data;

        P_Control player = FindObjectOfType<P_Control>();
        player.inputAxis = Vector2.zero;

        if (data.stageProgress[StageNumber - 1] < starNumber)
        {
            data.stageProgress[StageNumber - 1] = starNumber;
            SaveManager.Instance.SaveGameData();
        }
        StartCoroutine(UtillHelper.DelayedFunctionCall(CallIngameUI, 1f));
    }

    private void CallIngameUI()
    {
        IngameUI ingameUI = FindObjectOfType<IngameUI>();
        if (ingameUI != null)
            ingameUI.StageEnd(starNumber);
    }

    private void ResetValue()
    {
        sceneName = UtillHelper.GetCurrentSceneName();
        timer = 0;
        collectedCoin = 0;
        isPause = false;
    }

    private IEnumerator IStartStage()
    {
        ResetValue();
        while (SceneController.Instance.SceneChanging)
        {
            yield return null;
        }
        StartTimer();
    }

    public void StartStage()
    {
        StartCoroutine(IStartStage());
    }

    public override void Init()
    {
        if (sceneName == null)
            sceneName = UtillHelper.GetCurrentSceneName();
    }
}
