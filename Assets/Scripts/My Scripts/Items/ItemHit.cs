using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHit : MonoBehaviour
{
    // アイテムがプレイヤーに当たったときの処理を記述するスクリプト
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 例えば、アイテムを消す場合は以下のようにします
            Destroy(gameObject);
        }
    }
}
