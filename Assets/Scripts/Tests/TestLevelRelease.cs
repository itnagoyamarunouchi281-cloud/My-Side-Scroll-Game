using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLevelRelease : MonoBehaviour
{
    public static TestLevelRelease Instance;

    public bool AllLevelsCleared { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NotifyAllCleared()
    {
        AllLevelsCleared = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (AllLevelsCleared && scene.name == "Game Clear")
        {
            // ここでリリース処理、特殊UI表示、テスト用フラグ制御などを行います
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
