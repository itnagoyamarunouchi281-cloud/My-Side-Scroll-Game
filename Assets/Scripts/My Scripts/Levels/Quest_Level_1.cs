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

    void Start()
    {
        ResetScore();

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