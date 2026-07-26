using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Quest_Level_1 : IClearlable
{
    public int clearNum;
    public int levelNo;
    public Text enemyDeadText;

    [SerializeField] private CSVLoader cSVLoader;

    public static UnityEvent OnEnemyDestroyCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int enemyCounter;
    private int enemyNum = 1;
    private bool isGameClear;

    public int EnemyCounter {get => enemyCounter; }

    void Start()
    {
        ResetScore();
        LoadStageData();

        OnEnemyDestroyCountEvent.AddListener(() =>
        {
            AddScore(enemyNum);
        });

        OnGameClearEvent.AddListener(() =>
        {
            GameClear();
            GameClearSceneChange();
        });
    }

    private void Update()
    {
        if (!isGameClear)
        {
            if (clearNum <= enemyCounter)
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

        int lookupLevel = levelNo > 0 ? levelNo : clearNum;
        StageData stage = cSVLoader.GetStageDataByLevel(lookupLevel);
        if (stage != null)
        {
            clearNum = stage.Quest;
        }
        else
        {
            Debug.LogWarning($"No stage data found for level {lookupLevel}.");
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