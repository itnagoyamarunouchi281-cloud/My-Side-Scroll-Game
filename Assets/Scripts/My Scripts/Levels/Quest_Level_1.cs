using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Quest_Level_1 : IClearlable
{
    public int levelNo;
    public Text enemyDeadText;

    [SerializeField] private CSVLoader cSVLoader;

    public static UnityEvent OnEnemyDestroyCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int clearNum;
    private int enemyCounter;
    private int enemyNum = 1;
    private bool isGameClear;
    private bool isDataLoaded; // ★データがロード完了したか管理するフラグ

    public int EnemyCounter {get => enemyCounter; }

    private void OnQuestCount()
    {
        AddScore(enemyNum);
    }

    private void OnGameClear()
    {
        GameClear();
        GameClearSceneChange();
    }

    private void OnEnable()
    {
        OnEnemyDestroyCountEvent.AddListener(() =>
        {
            OnQuestCount();
        });
    }

    private void OnDisable()
    {
        OnEnemyDestroyCountEvent.RemoveListener(()=>
        {
            OnQuestCount();
        });

        OnGameClearEvent.RemoveListener(() =>
        {
            OnGameClear();
        });
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

        OnGameClearEvent.AddListener(() =>
        {
            OnGameClear();
        });
    }

    private void Update()
    {
        if (isGameClear == false)
        {
            if (isDataLoaded &&clearNum <= enemyCounter)
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
            Debug.Log("Quest: " + stage.Quest);
            clearNum = stage.Quest;
            isDataLoaded = true; // ★正しく読み込めたら判定許可フラグを立てる
        }
        else
        {
            Debug.LogWarning($"No stage data found for level {levelNo}.");
        }

        enemyDeadText.text = $"{EnemyData.EnemyType.ENEMY}:{enemyCounter} / {clearNum}";
    }

    private void ResetScore()
    {
        enemyCounter = 0;
        enemyDeadText.text = $"{EnemyData.EnemyType.ENEMY}:{enemyCounter} / {clearNum}";
    }

    private void AddScore(int point)
    {
        enemyCounter += point;
        enemyDeadText.text = $"{EnemyData.EnemyType.ENEMY}:{enemyCounter} / {clearNum}";
    }

    public int GetEnemyNum()
    {
        return enemyCounter;
    }
}