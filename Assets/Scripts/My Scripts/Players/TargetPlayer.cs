using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    Player playerPos;
    Vector3 vec;
    float distance;
    public int range = 5;
    private float speed = 5.0f;

    void Start()
    {
        playerPos = GameObject.Find("Actor").GetComponent<Player>();
    }

    void Update()
    {
        if (playerPos)
        {
            distance = Vector3.Distance(transform.position, playerPos.transform.position);
            vec = playerPos.transform.position - this.transform.position;
            vec = vec.normalized;
        }

        if (distance <= range)
        {
            transform.position += Time.deltaTime * vec * speed;
        }
    }
}
