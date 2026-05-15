using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCastomSpawn : MonoBehaviour
{
    [SerializeField] List<LevelCount> levelCounts;

    private List<GameObject> spawnedEnemies = new List<GameObject>();   // 生成済みの敵のリスト

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < levelCounts.Count; i++)
        {
            SpawnGameObjects(levelCounts[i]);
        }
    }

    private void SpawnGameObjects(LevelCount data)
    {
        if (data.isSpawn)
        {
            foreach (Vector3 p in data.pos)
            {
                // 敵を生成してリストに追加する
                GameObject enemyObj = Instantiate(
                    data.enemyPrefabs, p, Quaternion.identity, data.trans);
                spawnedEnemies.Add(enemyObj);
                enemyObj.SetActive(true);
            }
        }
    }

    public List<GameObject> GetSpawnedEnemies()
    {
        return spawnedEnemies;
    }
}

[System.Serializable]
public class LevelCount
{
    public enum Level
    {
        Level1,
        
        // これがラベルになる
    }

    public Level level;
    public List<Vector3> pos;
    public GameObject enemyPrefabs;
    public Transform trans;
    public bool isSpawn;
}
