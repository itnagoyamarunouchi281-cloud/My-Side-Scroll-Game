using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Quest_Level_1 : IClearlable
{
    public int clearNum;
    public Text enemyDeadText;

    public static UnityEvent OnEnemyDestroyCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();
    private int enemyCounter;
    private int enemyNum = 1;
    private bool isGameClear;

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

        OnGameClearEvent.AddListener(() =>
        {
            OnGameClear();
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

    void Start()
    {
        ResetScore();
    }

    private void Update()
    {
        if (isGameClear == false)
        {
            if (clearNum <= enemyCounter)
            {
                OnGameClearEvent.Invoke();
                isGameClear = true;
            }
        }
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