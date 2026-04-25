using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Coin_Level1 : IClearlable
{
    public int clearNum;
    public Text coinAddText;
    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();
    private int itemCounter;
    private bool isGameClear;

    void Start()
    {
        ResetScore();

        OnCoinCountEvent.AddListener(() =>
        {
            AddScore(1);
        });

        OnGameClearEvent.AddListener(() =>
        {
            GameClear();
            GameClearSceneChange();
        });
    }

    void Update()
    {
        if (!isGameClear)
        {
            if (clearNum <= itemCounter)
            {
                OnGameClearEvent.Invoke();
                isGameClear = true;
            }
        }
    }

    private void ResetScore()
    {
        itemCounter = 0;
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{itemCounter} / {clearNum}";
    }

    private void AddScore(int point)
    {
        itemCounter += point;
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{itemCounter} / {clearNum}";
    }
}
