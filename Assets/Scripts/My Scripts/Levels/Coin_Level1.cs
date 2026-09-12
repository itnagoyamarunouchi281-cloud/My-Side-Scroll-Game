using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Quest_Level1 : IClearlable
{
    [Header("目標設定")]
    [Tooltip("クリアに必要なコインの数")]
    public int clearCoinNum;
    [Tooltip("クリアに必要な撃破敵数")]
    public int clearEnemyNum;

    [Header("UI表示")]
    public Text questProgressText;

    // イベント定義
    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnEnemyDefeatedEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int currentEnemyCount = 0;
    private bool isGameClear = false;

    private void OnEnable()
    {
        OnCoinCountEvent.AddListener(OnCoinCount);
        OnEnemyDefeatedEvent.AddListener(OnEnemyDefeated);
    }

    private void OnDisable()
    {
        OnCoinCountEvent.RemoveListener(OnCoinCount);
        OnEnemyDefeatedEvent.RemoveListener(OnEnemyDefeated);
        OnGameClearEvent.RemoveListener(OnGameClear);
    }

    void Start()
    {
        ResetProgress();
        OnGameClearEvent.AddListener(OnGameClear);
    }

    void Update()
    {
        if (!isGameClear)
        {
            // コイン全取得 AND 敵全滅 の両方を満たしたか判定
            bool isCoinCleared = GameManager.Instance.scoreNum >= clearCoinNum;
            bool isEnemyCleared = currentEnemyCount >= clearEnemyNum;

            if (isCoinCleared && isEnemyCleared)
            {
                isGameClear = true;
                OnGameClearEvent.Invoke();
            }
        }
    }

    private void OnCoinCount()
    {
        AddCoin(1);
    }

    private void OnEnemyDefeated()
    {
        AddEnemyCount(1);
    }

    private void OnGameClear()
    {
        GameClear();
        GameClearSceneChange();
    }

    private void ResetProgress()
    {
        GameManager.Instance.scoreNum = 0;
        currentEnemyCount = 0;
        UpdateUI();
    }

    private void AddCoin(int point)
    {
        GameManager.Instance.scoreNum += point;
        UpdateUI();
    }

    private void AddEnemyCount(int count)
    {
        currentEnemyCount += count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (questProgressText != null)
        {
            questProgressText.text = $"COIN: {GameManager.Instance.scoreNum} / {clearCoinNum}\n" +
                                     $"ENEMY: {currentEnemyCount} / {clearEnemyNum}";
        }
    }
}