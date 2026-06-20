using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Coin_Level1 : IClearlable
{
    public int clearNum;
    public Text coinAddText;
    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private const string LastScoreKey = "LastScore";

    private int itemCounter;
    private bool isGameClear;

    public int GetItemCounter()
    {
        return itemCounter;
    }

    private void SaveLastScore()
    {
        PlayerPrefs.SetInt(LastScoreKey, itemCounter);
        PlayerPrefs.Save();
    }

    private void OnCoinCount()
    {
        AddScore();
    }

    private void OnGameClear()
    {
        GameClear();
        SaveLastScore();
        GameClearSceneChange();
    }

    private void OnEnable()
    {
        OnCoinCountEvent.AddListener(OnCoinCount);
        OnGameClearEvent.AddListener(OnGameClear);
    }

    private void OnDisable()
    {
        OnCoinCountEvent.RemoveListener(OnCoinCount);
        OnGameClearEvent.RemoveListener(OnGameClear);
    }

    void Start()
    {
        ResetScore();
    }

    void Update()
    {
        if (!isGameClear && clearNum <= itemCounter)
        {
            OnGameClearEvent.Invoke();
            isGameClear = true;
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
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{itemCounter} / {clearNum}";
    }
}
