using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryOnYDecrement : MonoBehaviour
{
    public float retryYThreshold = -10f; // リトライするY軸のしきい値

    void Update()
    {
        // Y軸がしきい値を下回ったらリトライ
        if (transform.position.y < retryYThreshold)
        {
            RetryGame();
        }
    }

    void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
