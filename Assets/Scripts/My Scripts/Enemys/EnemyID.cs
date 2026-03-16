using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public int sum; // シーン内の全ての敵キャラの加算用 1,1,1,…
    public static int enemyCount = 0;  // 固有番号をつける
    public string enemyName;

    private int enemyId;

    void Awake()
    {
        enemyId = enemyCount++;
    }
}
