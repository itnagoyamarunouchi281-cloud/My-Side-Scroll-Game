using System;
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

    public int GetItemCounter()
    {
        return itemCounter;
    }
    
    private void GameClearScene()
    {
        PlayerPrefs.SetInt("LastScore", itemCounter);
        PlayerPrefs.Save();
    } 

    private void OnCoinCount()
    {
        AddScore();
    }

    private void OnGameClear()
    {
        GameClear();
        GameClearScene();
        GameClearSceneChange();
    }

    private void OnEnable()
    {
        OnCoinCountEvent.AddListener(OnCoinCount);
        OnGameClearEvent.AddListener(OnGameClear);
    }

    void Start()
    {
        ResetScore();
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

    private void AddScore()
    {
        itemCounter += GameManager.Instance.CurrentScore;
        Debug.Log($"aaaaaa{itemCounter}");
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{itemCounter} / {clearNum}";
    }
}
