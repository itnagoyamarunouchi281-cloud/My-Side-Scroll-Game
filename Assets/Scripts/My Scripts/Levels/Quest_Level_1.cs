using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Quest_Level_1 : MonoBehaviour
{
    public int clearNum;
    public Text coinAddText;

    public static UnityEvent OnEnemyDestroyCountEvent = new UnityEvent();

    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private int enemyCounter;
    private bool isGameClear;

    void Start()
    {
        ResetScore();

        OnEnemyDestroyCountEvent.AddListener(() =>
        {
            AddScore(1);
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
        coinAddText.text = $"{EnemyData.EnemyType.TYPE1}:{enemyCounter} / {clearNum}";
    }

    private void AddScore(int point)
    {
        enemyCounter += point;
        coinAddText.text = $"{EnemyData.EnemyType.TYPE1}:{enemyCounter} / {clearNum}";
    }

    public void GameClear()
    {
        StaticClear.AddClearNum();
    }

    public void GameClearSceneChange()
    {
        FadeManager.Instance.LoadScene("Game Clear", 2.0f);   
    }
}