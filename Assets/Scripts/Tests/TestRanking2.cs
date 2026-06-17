using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestRanking2 : MonoBehaviour
{
    public const string SCENENAME = "ClearScene";
    private const string RANKING_PREF_KEY = "ranking";
    private const int RANKING_NUM = 10;
    public float[] Ranking { get; private set; } = new float[RANKING_NUM];

    [SerializeField] private GUIStyle rankingLabelStyle;

    private float totalPlayTime = 0f;
    private const string SAVE_KEY = "TotalPlayTime";

    private void OnEnable()
    {
        Coin_Level1.OnGameClearEvent.AddListener(() =>
        {
            SaveRanking(totalPlayTime);
        });
    }

    private void OnDisable()
    {
        Coin_Level1.OnGameClearEvent.RemoveListener(() =>
        {
            SaveRanking(totalPlayTime);
        });
    }

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == SCENENAME)
        {
            LoadRanking();
        }
    }

    private void Start()
    {
        // 既存のプレイ時間を読み込む（保存されていない場合は0）
        totalPlayTime = PlayerPrefs.GetFloat(SAVE_KEY, 0f);
        Debug.Log("これまでのプレイ時間: " + totalPlayTime + " 秒");
    }

    private void Update()
    {
        totalPlayTime += Time.deltaTime;
    }

    private void LoadRanking()
    {
        string rawRanking = PlayerPrefs.GetString(RANKING_PREF_KEY, string.Empty);
        if (string.IsNullOrEmpty(rawRanking))
        {
            return;
        }

        string[] scoreStrings = rawRanking.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < scoreStrings.Length && i < RANKING_NUM; i++)
        {
            if (float.TryParse(scoreStrings[i].Trim(), out float parsedScore))
            {
                Ranking[i] = parsedScore;
            }
            else
            {
                Ranking[i] = 0f;
            }
        }
    }

    public void SaveRanking(float newScore)
    {
        float[] newRanking = new float[RANKING_NUM];
        Ranking.CopyTo(newRanking, 0);

        for (int i = 0; i < RANKING_NUM; i++)
        {
            if (newScore > newRanking[i])
            {
                float temp = newRanking[i];
                newRanking[i] = newScore;
                newScore = temp;
            }
        }

        Ranking = newRanking;
        string rankingString = string.Join(",", Array.ConvertAll(Ranking, value => value.ToString()));
        PlayerPrefs.SetString(RANKING_PREF_KEY, rankingString);
        PlayerPrefs.Save();
    }

    public void DeleteRanking()
    {
        PlayerPrefs.DeleteKey(RANKING_PREF_KEY);
        Ranking = new float[RANKING_NUM];
    }

    private void OnGUI()
    {
        if (rankingLabelStyle == null)
        {
            return;
        }

        Rect rectRanking = new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f);
        string rankingString = GetRankingDisplayText();
        GUI.Label(rectRanking, rankingString, rankingLabelStyle);
    }

    private string GetRankingDisplayText()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < Ranking.Length; i++)
        {
            builder.AppendFormat("{0}位 {1:F2}秒\n", i + 1, Ranking[i]);
        }
        return builder.ToString();
    }

    // シーン遷移時など、定期的にセーブしたい場合にも呼び出せます
    public void SavePlayTime()
    {
        PlayerPrefs.SetFloat(SAVE_KEY, totalPlayTime);
        PlayerPrefs.Save(); // 確実に即時保存
        Debug.Log("セーブしたプレイ時間: " + totalPlayTime + " 秒");
    }
}
