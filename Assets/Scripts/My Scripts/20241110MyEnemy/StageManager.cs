using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class StageManager : MonoBehaviour
{
    public Image bossHPGage;

    public AreaManager initArea;

    public AudioClip bossBGMClip;

    private AreaManager[] inStageAreas;

    private int currentStage = 0;

    // Start
    void Start()
    {
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);

        initArea.ActiveArea();

        bossHPGage.transform.parent.gameObject.SetActive(false);
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