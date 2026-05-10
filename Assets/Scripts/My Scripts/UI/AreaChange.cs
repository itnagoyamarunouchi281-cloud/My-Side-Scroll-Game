using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChange : MonoBehaviour
{
    public StageManager stageManager;
    public int nextStageIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stageManager.ChangeStage(nextStageIndex);
        }
    }
}
