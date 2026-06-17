using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    const int Num = (int)EnemyData.EnemyType.MAX_ENEMY;

    int[] Storage = new int[Num];

    bool fadestart = false;

    void Start()
    {
        for (int i = 0; i < Storage.Length; ++i)
        {
            Storage[i] = 0;
        }
    }


    void Update()
    {
        StaticEnemy.IsUpdate = false;
    }

    public void AddEnemy(EnemyData.EnemyType type)
    {
        Storage[(int)type]++;
        Debug.Log($"Storage: {Storage[(int)type]}");
    }

    public int[] GetStorage()
    {
        return Storage;
    }
}
