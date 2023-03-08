using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DataHelper : Singleton<DataHelper>
{
    public TextAsset stageDataCSV;

    private List<Dictionary<string, string>> stageData = null;
    public List<Dictionary<string, string>> StageData
    {
        get
        {
            if(stageData == null)
                stageData = LoadCSV(stageDataCSV.text);
            return stageData;
        }
    }

    public void Awake()
    {
        DataHelper[] temp = FindObjectsOfType<DataHelper>();
        if (temp.Length > 1)
            Destroy(this.gameObject);
        else
        {
            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(this.transform.root.gameObject);
            else
                DontDestroyOnLoad(this.gameObject);
            stageData = LoadCSV(stageDataCSV.text);
            instance = this;
        }
    }

    public string GetStageSubTitle(int stageNumber)
    {
        if (stageData == null || StageData.Count < stageNumber)
            return "";

        string subTitle = StageData[stageNumber - 1]["SubTitle"].ToString();
        return subTitle;
    }

    public float GetStageLimitTime(int stageNumber)
    {
        if (stageData == null || StageData.Count < stageNumber)
            return 0;

        string limitTimeData = StageData[stageNumber - 1]["LimitTime"].ToString();
        float limitTime = 0;
        if (float.TryParse(limitTimeData, out limitTime))
            return limitTime;
        else
            return 0;
    }

    public int GetStageMaxCoin(int stageNumber)
    {
        if (stageData == null || StageData.Count < stageNumber)
            return 0;

        string maxCoinData = StageData[stageNumber - 1]["CoinNumber"].ToString();
        int maxCoin = 0;
        if (int.TryParse(maxCoinData, out maxCoin))
            return maxCoin;
        else
            return 0;
    }

    private List<Dictionary<string, string>> LoadCSV(string str)
    {
        string[] lines = str.Split('\n');

        string[] heads = lines[0].Split(',');


        heads[heads.Length - 1] = heads[heads.Length - 1].Replace("\r", "");

        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] col = lines[i].Split(',');
            col[col.Length - 1] = col[col.Length - 1].Replace("\r", "");
            for (int j = 0; j < heads.Length; j++)
            {
                string value = col[j];

                dic.Add(heads[j], col[j]);
            }

            list.Add(dic);
        }

        return list;
    }

}
