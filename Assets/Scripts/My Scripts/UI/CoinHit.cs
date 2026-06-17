using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHit : MonoBehaviour
{
    [SerializeField] GameObject effectObj;

    private void OnTriggerEnter(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(effectObj, obj.transform.position, Quaternion.identity);
            Coin_Level1.OnCoinCountEvent.Invoke();
            SoundManager.Instance.PlaySE(SESoundData.SE.HITSE);
            Destroy(gameObject);
        }
    }
}
