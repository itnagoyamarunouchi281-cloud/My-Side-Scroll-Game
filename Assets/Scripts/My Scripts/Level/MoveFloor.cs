using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    public float timerA;

    [SerializeField] private float moveRange;
    [SerializeField] private float dir;

    private Transform thistrans;
    private Vector3 pos;
    private Vector3 initPos;
    
    void Start()
    {
        dir = 1;
        initPos = this.transform.position;
        thistrans = transform;
    }

    void Update()
    {
        pos = thistrans.position;

        // 常に移動させる（timerA を速度として使用）
        pos.y += dir * timerA * Time.deltaTime;

        float upper = initPos.y + moveRange;
        float lower = initPos.y - moveRange;

        // 範囲を超えたら位置を補正して方向を反転
        if (pos.y > upper)
        {
            pos.y = upper;
            dir = -1;
        }
        else if (pos.y < lower)
        {
            pos.y = lower;
            dir = 1;
        }

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
