using UnityEngine;
using UnityEngine.UI;

public class EnemyNum : MonoBehaviour
{
    public GameObject[] DispNum = null;
    public int[] enemyIndex;

    [SerializeField] private MyEnemy myEnemy;

    private Text[] enemyNum = new Text[(int)EnemyData.EnemyType.MAX_ENEMY];

    void Start()
    {
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
        enemyNum[0].text = $"{EnemyData.EnemyType.TYPE1}" + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
    }
}
