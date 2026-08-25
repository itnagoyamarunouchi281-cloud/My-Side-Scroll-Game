using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int scoreNum;
    public int levelNum;

    private CSVLoader cSVLoader;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (cSVLoader == null)
        {
            cSVLoader = FindObjectOfType<CSVLoader>();
        }

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

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("現在のシーン番号: " + sceneIndex);
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

        StageData stage = cSVLoader.GetStageDataByLevel(levelNum);
        if (stage != null)
        {
            Debug.Log("Level: " + stage.Level);
            levelNum = stage.Level;
        }
        else
        {
            Debug.LogWarning($"No stage data found for level {levelNum}.");
        }
    }
}
