using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Quest_Level_1.OnEnemyDestroyCountEvent.Invoke();
            SoundManager.Instance.PlaySE(SESoundData.SE.ItemGet);
            Destroy(gameObject);
        }
    }
}
