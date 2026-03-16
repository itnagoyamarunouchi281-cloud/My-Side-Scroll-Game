using UnityEngine;
using UnityEngine.UI;

public class EnemyNum : MonoBehaviour
{
    // 敵ごとに数を表示するGameObject配列
    public GameObject[] DispNum = null;
    public int[] enemyIndex;

    [SerializeField] private MyEnemy myEnemy;

    private Text[] enemyNum = new Text[(int)EnemyData.EnemyType.MAX_ENEMY];

    void Start()
    {
        // オブジェクトからTextコンポーネントを取得
        for (int i = 0; i < DispNum.Length; i++)
        {
            enemyNum[i] = DispNum[i].GetComponent<Text>();
        }

        /*
        enemyNum[0] = DispNum[0].GetComponent<Text>();
        enemyNum[1] = DispNum[1].GetComponent<Text>();
        enemyNum[2] = DispNum[2].GetComponent<Text>();
        enemyNum[3] = DispNum[3].GetComponent<Text>();
        enemyNum[4] = DispNum[4].GetComponent<Text>();
        enemyNum[5] = DispNum[5].GetComponent<Text>();
        */
    }

    void Update()
    {
        // テキストの表示を入れ替える
        enemyNum[0].text = $"{EnemyData.EnemyType.マ一号} " + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
    }
}
