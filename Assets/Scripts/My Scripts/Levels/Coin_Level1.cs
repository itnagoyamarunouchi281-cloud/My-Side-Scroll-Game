using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Coin_Level1 : IClearlable
{
    public int levelNo;
    public Text coinAddText;

    [SerializeField] private CSVLoader cSVLoader;

    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int clearNum;
    private int coinNum = 1;
    private bool isGameClear;
    private bool isDataLoaded; // ★データがロード完了したか管理するフラグ

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
    }

    private void OnDisable()
    {
        OnCoinCountEvent.RemoveListener(OnCoinCount);
        OnGameClearEvent.RemoveListener(OnGameClear);
    }

    private void OnDestroy()
    {
        // イベント登録の解除（メモリリーク防止）
        if (cSVLoader != null)
        {
            cSVLoader.OnLoaded -= LoadStageData;
        }
    }

    void Start()
    {
        ResetScore();
        
        if (cSVLoader != null)
        {
            // すでに読み込み済み（リストにデータがある）なら即実行
            if (cSVLoader.stageList != null && cSVLoader.stageList.Count > 0)
            {
                LoadStageData();
            }
            else
            {
                // まだ読み込み中なら、完了イベントに登録しておく
                cSVLoader.OnLoaded += LoadStageData;
            }
        }
        else
        {
            Debug.LogWarning("CSVLoader is not assigned or found.");
        }

        OnGameClearEvent.AddListener(OnGameClear);
    }

    void Update()
    {
        if(isGameClear == false)
        {
            if (isDataLoaded && clearNum <= GameManager.Instance.scoreNum)
            {
                OnGameClearEvent.Invoke();
                isGameClear = true;
            }
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

        StageData stage = cSVLoader.GetStageDataByLevel(levelNo);
        if (stage != null)
        {
            Debug.Log("Coin: " + stage.Coin);
            clearNum = stage.Coin;
            isDataLoaded = true; // ★正しく読み込めたら判定許可フラグを立てる
        }
        else
        {
            Debug.LogWarning($"No stage data found for level {levelNo}.");
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
