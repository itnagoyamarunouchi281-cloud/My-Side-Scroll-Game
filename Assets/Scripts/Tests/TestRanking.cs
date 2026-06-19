using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestRanking : MonoBehaviour
{
    // =============================================
    // ランキング更新・取得処理
    // いるもの
    // ・5回まで
    // ・クリアした回数
    // #
    // StaticClear.ClearNum
    // 
    // =============================================
    public static TestRanking Instance;

    [SerializeField] private Text[] text_ScoreRanking;

    public int highScore;

    private const string RankingKey = "testRanking";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCoinCount()
    {
        highScore = GameManager.Instance.CurrentScore;
        AddScore(highScore);
    }

    private void OnEnable()
    {
        Coin_Level1.OnCoinCountEvent.AddListener(OnCoinCount);
    }

    private void OnDisable()
    {
        Coin_Level1.OnCoinCountEvent.RemoveListener(OnCoinCount);
    }

    void Start()
    {
        var ranking = LoadRanking();

        int count = Mathf.Min(ranking.Count, text_ScoreRanking.Length);

        for (int i = 0; i < count; i++)
        {
            text_ScoreRanking[i].text = $"{i + 1}位 : {ranking[i]}";
        }
    }

    public void AddScore(int score)
    {
        List<int> scores = LoadRanking();

        scores.Add(score);

        scores = scores
            .OrderByDescending(x => x)
            .Take(5)
            .ToList();

        PlayerPrefs.SetString(
            RankingKey,
            string.Join(",", scores)
        );
    }

    public List<int> LoadRanking()
    {
        string text =
            PlayerPrefs.GetString(
                RankingKey,
                "500,400,300,200,100"
            );

        return text.Split(',')
                   .Select(int.Parse)
                   .ToList();
    }
}
