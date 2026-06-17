using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyAttack : MonoBehaviour
{
    private EnemyBase enemyBase;

    // Start
    void Start()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    private void OnTriggerStay(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {
            enemyBase.BodyAttack(collision.gameObject);
        }
    }
}