using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestRanking : MonoBehaviour
{
    [SerializeField] private Text[] scoreRankingTexts;

    private const string RankingKey = "ScoreRanking";
    private const string LastScoreKey = "LastScore";
    private const int MaxRankingCount = 5;
    private static readonly int[] DefaultRanking = { 50, 40, 30, 20, 10 };

    private void Start()
    {
        AddLastScoreToRanking();
        DisplayRanking();
    }

    private void AddLastScoreToRanking()
    {
        if (!PlayerPrefs.HasKey(LastScoreKey))
        {
            return;
        }

        int lastScore = PlayerPrefs.GetInt(LastScoreKey);
        AddScore(lastScore);
        PlayerPrefs.DeleteKey(LastScoreKey);
    }

    private void DisplayRanking()
    {
        var ranking = LoadRanking();
        int count = Mathf.Min(ranking.Count, scoreRankingTexts.Length);

        for (int i = 0; i < scoreRankingTexts.Length; i++)
        {
            string label = i < count ? $"{i + 1}位 : {ranking[i]}" : $"{i + 1}位 : ---";
            scoreRankingTexts[i].text = label;
        }
    }

    public void AddScore(int score)
    {
        var scores = LoadRanking();
        scores.Add(score);

        scores = scores
            .OrderByDescending(x => x)
            .Take(MaxRankingCount)
            .ToList();

        SaveRanking(scores);
    }

    public List<int> LoadRanking()
    {
        string storedRanking = PlayerPrefs.GetString(RankingKey, string.Join(",", DefaultRanking));
        if (string.IsNullOrWhiteSpace(storedRanking))
        {
            return new List<int>(DefaultRanking);
        }

        var scores = storedRanking
            .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(token => int.TryParse(token, out int value) ? value : (int?)null)
            .Where(value => value.HasValue)
            .Select(value => value.Value)
            .ToList();

        return scores.Count > 0 ? scores.Take(MaxRankingCount).ToList() : new List<int>(DefaultRanking);
    }

    private void SaveRanking(IReadOnlyList<int> scores)
    {
        PlayerPrefs.SetString(RankingKey, string.Join(",", scores));
        PlayerPrefs.Save();
    }
}
