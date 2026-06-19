using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField] private float timerA;
    [SerializeField] private float dir;
    [SerializeField] private float moveRange = 7.0f;

    private Transform thistrans;
    private Vector3 pos;
    private Vector3 initPos;

    void Start()
    {
        //StartCoroutine(MoveTask());
        dir = 1;
    }

    void Update()
    {
        thistrans = this.transform;
        pos = thistrans.position;
        
        if (thistrans.position.y > initPos.y + moveRange)
        {
            dir = -1;
        }

        if (thistrans.position.y < initPos.y - moveRange)
        {
            dir = 1;
        }

        pos.y += dir * Time.deltaTime;
        thistrans.position = pos;
    }

    IEnumerator MoveTask()
    {
        MoveUp(timerA);
        yield return new WaitForSeconds(2f);
        MoveDown(-timerA);
    }

    public void MoveUp(float posY)
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }

    public void MoveDown(float posY)
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }

    public void MoveLeft(float posX)
    {
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    public void MoveRight(float posX)
    {
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
