using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージ管理クラス - エリアの自動進行機能付き
/// </summary>
public class StageManager : MonoBehaviour
{
    public Image bossHPGage;

    public AreaManager initArea;

    public AudioClip bossBGMClip;

    // 自動進行設定
    [Header("自動進行設定")]
    [Tooltip("エリア移動までの秒数（0で自動進行無効）")]
    public float autoProgressionInterval;

    [Tooltip("エリア移動前の警告秒数")]
    public float warningTime;

    private AreaManager[] inStageAreas;

    private int currentStage = 0;

    // 自動進行用
    private Coroutine autoProgressionCoroutine;
    private bool isAutoProgressionActive = false;

    // Start
    void Start()
    {
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);

        initArea.ActiveArea();

        bossHPGage.transform.parent.gameObject.SetActive(false);

        // 自動進行が有効な場合、開始
        if (autoProgressionInterval > 0f)
        {
            StartAutoProgression();
        }
    }

    /// <summary>
    /// 自動進行を開始
    /// </summary>
    public void StartAutoProgression()
    {
        if (autoProgressionInterval <= 0f) return;
        
        if (autoProgressionCoroutine != null)
        {
            StopCoroutine(autoProgressionCoroutine);
        }
        
        isAutoProgressionActive = true;
        autoProgressionCoroutine = StartCoroutine(AutoProgressionLoop());
    }

    /// <summary>
    /// 自動進行を停止
    /// </summary>
    public void StopAutoProgression()
    {
        isAutoProgressionActive = false;
        if (autoProgressionCoroutine != null)
        {
            StopCoroutine(autoProgressionCoroutine);
            autoProgressionCoroutine = null;
        }
    }

    /// <summary>
    /// 自動進行ループ（コルーチン）
    /// </summary>
    private IEnumerator AutoProgressionLoop()
    {
        while (isAutoProgressionActive && currentStage < inStageAreas.Length - 1)
        {
            // 指定した秒数만큼待機
            yield return new WaitForSeconds(autoProgressionInterval);

            if (!isAutoProgressionActive) yield break;

            // 次のエリアへ遷移
            ProceedToNextArea();
        }

        // 最後のエリアに達した場合、自动進行を停止
        if (currentStage >= inStageAreas.Length - 1)
        {
            Debug.Log("最後のエリアに達しました。自動進行を停止します。");
            isAutoProgressionActive = false;
        }
    }

    /// <summary>
    /// 次のエリアへ遷移
    /// </summary>
    public void ProceedToNextArea()
    {
        int nextStage;
        
        currentStage++;
        nextStage = currentStage;
        
        if (nextStage < inStageAreas.Length)
        {
            ChangeStage(nextStage);
            Debug.Log($"自動進行: エリア {currentStage} → {nextStage}");
        }
        else
        {
            Debug.Log("最後のエリアです。");
            StopAutoProgression();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void DeactivateAllAreas()
    {
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayBossBGM()
    {
        GetComponent<AudioSource>().clip = bossBGMClip;
        GetComponent<AudioSource>().Play();
    }

    public void ChangeStage(int nextStage)
    {
        currentStage = nextStage;
        Debug.Log($"Stage changed to {currentStage}");

        // 指定したインデックスのエリアのみをアクティブ化
        if (currentStage >= 0 && currentStage < inStageAreas.Length)
        {
            inStageAreas[currentStage].ActiveArea();
        }
        else
        {
            Debug.LogWarning($"Invalid stage index: {currentStage}");
        }
    }

    public void StageClear()
    {

    }

    public void StageOver()
    {
        
    }
}