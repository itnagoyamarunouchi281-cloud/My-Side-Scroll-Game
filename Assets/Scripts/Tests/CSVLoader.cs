using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CSVLoader : MonoBehaviour
{
    public string csvURL =
        "https://docs.google.com/spreadsheets/d/1nhtxfbF2W1FIY_DHkq4j8ZYVO9KgHHdID8jWsj_HEJU/export?format=csv";

    public List<StageData> stageList = new();

    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(csvURL);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ParseCSV(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public StageData GetStageDataByLevel(int levelNumber)
    {
        if (stageList == null || stageList.Count == 0)
        {
            return null;
        }

        return stageList.Find(x => x != null && x.Level == levelNumber);
    }

    void ParseCSV(string csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
        {
            return;
        }

        string[] lines = csv.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        if (lines.Length == 0)
        {
            return;
        }

        string[] headers = lines[0].Split(',');
        stageList.Clear();

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] cols = lines[i].Split(',');
            StageData data = new StageData();

            data.Level = ParseInt(GetColumnValue(headers, cols, "Level", "Stage", "StageNo"));
            if (data.Level <= 0)
            {
                data.Level = stageList.Count + 1;
            }

            data.Quest = ParseInt(GetColumnValue(headers, cols, "Quest", "Enemy", "EnemyCount", "KillCount"));
            data.Coin = ParseInt(GetColumnValue(headers, cols, "Coin", "CoinCount", "Collect", "CoinGoal"));

            if (data.Level <= 0 && data.Quest <= 0 && data.Coin <= 0)
            {
                continue;
            }

            stageList.Add(data);
        }

        Debug.Log(stageList.Count + "件読み込みました");
    }

    private string GetColumnValue(string[] headers, string[] row, params string[] aliases)
    {
        if (headers == null || row == null)
        {
            return string.Empty;
        }

        for (int headerIndex = 0; headerIndex < headers.Length; headerIndex++)
        {
            if (headerIndex >= row.Length)
            {
                break;
            }

            string headerName = NormalizeHeader(headers[headerIndex]);
            foreach (string alias in aliases)
            {
                if (string.IsNullOrEmpty(alias))
                {
                    continue;
                }

                string normalizedAlias = NormalizeHeader(alias);
                if (headerName == normalizedAlias || headerName.Contains(normalizedAlias) || normalizedAlias.Contains(headerName))
                {
                    return row[headerIndex].Trim();
                }
            }
        }

        return string.Empty;
    }

    private int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }

        return 0;
    }

    private string NormalizeHeader(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (char c in value.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(c))
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}