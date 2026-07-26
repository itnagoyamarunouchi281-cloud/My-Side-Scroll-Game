using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Coin_Level1 : IClearlable
{
    public int clearNum;
    public int levelNo;
    public Text coinAddText;

    [SerializeField] private CSVLoader cSVLoader;

    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int coinNum = 1;
    private bool isGameClear;

    private void OnCoinCount()
    {
        AddScore(coinNum);
    }

    private void OnGameClear()
    {
        GameClear();
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
        LoadStageData();
    }

    void Update()
    {
        if (!isGameClear && clearNum <= GameManager.Instance.scoreNum)
        {
            OnGameClearEvent.Invoke();
            isGameClear = true;
        }
    }

    private void LoadStageData()
    {
        if (cSVLoader == null)
        {
            cSVLoader = FindObjectOfType<CSVLoader>();
        }

        if (cSVLoader == null)
        {
            Debug.LogWarning("CSVLoader is not assigned or found.");
            return;
        }

        if (cSVLoader.stageList == null || cSVLoader.stageList.Count == 0)
        {
            Debug.LogWarning("CSVLoader stage data is empty.");
            return;
        }

        int lookupLevel = levelNo > 0 ? levelNo : clearNum;
        StageData stage = cSVLoader.GetStageDataByLevel(lookupLevel);
        if (stage != null)
        {
            clearNum = stage.Coin;
        }
        else
        {
            Debug.LogWarning($"No stage data found for level {lookupLevel}.");
        }

        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{GameManager.Instance.scoreNum} / {clearNum}";
    }

    private void ResetScore()
    {
        GameManager.Instance.scoreNum = 0;
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{GameManager.Instance.scoreNum} / {clearNum}";
    }

    private void AddScore(int point)
    {
        GameManager.Instance.scoreNum += point;
        coinAddText.text = $"{EnemyData.EnemyType.COIN}:{GameManager.Instance.scoreNum} / {clearNum}";
    }
}
