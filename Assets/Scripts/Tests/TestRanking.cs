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

    [SerializeField] private Text[] text_Score;

    private int highScore;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int point)
    {
        highScore += point;
    }

    public int GetScore()
    {
        return highScore;
    }

    void Start()
    {
        // 初期のランキングの出力
        RankingLoad("testRanking");

        // ランキング更新
        RankingUpdate("testRanking", highScore);

        // 更新後のランキングの出力
        RankingLoad("testRanking");
    }

    // =============================================
    // ランキング処理に関わるメソッド
    // =============================================

    // ランキング更新メソッド
    void RankingUpdate(string rankingKey, int newScore)
    {
        List<int> rankingScores = GetTopHighScores(rankingKey, 5);
        rankingScores.Add(newScore);

        rankingScores = rankingScores
            .OrderByDescending(score => score)
            .Take(5)
            .ToList();

        PlayerPrefs.SetString(rankingKey, string.Join(",", rankingScores));
    }

    // ランキング取得メソッド
    void RankingLoad(string rankingKey)
    {
        List<int> rankingScores = GetTopHighScores(rankingKey, 5);

        for (int i = 0; i < rankingScores.Count; i++)
        {
            text_Score[i].text = $"{i + 1}位：{rankingScores[i]}";
        }
    }

    // topN のスコアを高い順に取得してソートする
    List<int> GetTopHighScores(string rankingKey, int topCount)
    {
        string rankingText = PlayerPrefs.GetString(rankingKey, "500,400,300,200,100");

        return rankingText
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(text => int.TryParse(text.Trim(), out int score) ? score : 0)
            .OrderByDescending(score => score)
            .Take(topCount)
            .ToList();
    }
}
