using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    //オブジェクトを現在座標で上下移動で往復させるためのコードが以下です。
    void Start()
    {
        StartCoroutine(MoveTask());
    }

    // 上下移動のコルーチン
    // 使いどころ
    // キャラクターが床に乗った時にだけ上に移動したい場合など
    IEnumerator MoveTask()
    {
        MoveUp(3.5f);
        yield return new WaitForSeconds(2f);
        MoveDown(-3.5f);
    }

    void MoveUp(float posY)
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }

    void MoveDown(float posY)
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}
