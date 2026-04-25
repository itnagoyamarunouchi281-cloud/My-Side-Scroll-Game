using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Coin_Level1.OnCoinCountEvent.Invoke();
            SoundManager.Instance.PlaySE(SESoundData.SE.ItemGet);
            Destroy(gameObject);
        }
    }
}
