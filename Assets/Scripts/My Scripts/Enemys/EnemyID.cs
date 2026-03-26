using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public int sum;
    public static int enemyCount = 0;
    public string enemyName;

    private int enemyId;

    void Awake()
    {
        enemyId = enemyCount++;
    }
}
