using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(MoveTask());
    }

    IEnumerator MoveTask()
    {
        MoveUp(3.5f);
        yield return new WaitForSeconds(2f);
        MoveDown(-3.5f);
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
