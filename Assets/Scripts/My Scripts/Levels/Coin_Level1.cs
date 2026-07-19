using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Coin_Level1 : IClearlable
{
    public int clearNum;
    public Text coinAddText;
    public static UnityEvent OnCoinCountEvent = new UnityEvent();
    public static UnityEvent OnGameClearEvent = new UnityEvent();

    private bool isGameClear;

    private void OnCoinCount()
    {
        AddScore(1);
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
    }

    void Update()
    {
        if (!isGameClear && clearNum <= GameManager.Instance.scoreNum)
        {
            OnGameClearEvent.Invoke();
            isGameClear = true;
        }
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
