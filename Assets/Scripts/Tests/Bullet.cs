using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStatus enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();
            if (enemyStatus != null)
            {
                enemyStatus.SetHp(10);
            }
        }
        Destroy(gameObject);
    }
}
