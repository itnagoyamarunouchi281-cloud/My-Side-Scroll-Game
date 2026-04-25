using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2Hit : MonoBehaviour
{
    [SerializeField] private int nextStage = 1;

    private void Update()
    {
        transform.Rotate(0, 0f, 90f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Quest_Level_1.OnEnemyDestroyCountEvent.Invoke();
            SEManager.instance.PlaySE(0);

            nextStage++;

            StageManager stageManager = FindObjectOfType<StageManager>();
            if (stageManager != null)
            {
                stageManager.ChangeStage(nextStage);
            }

            Destroy(gameObject);
        }
    }
}
