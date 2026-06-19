using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IClearlable : MonoBehaviour
{
    public void GameClear()
    {
        StaticClear.AddClearNum();
    }

    public void GameClearSceneChange()
    {
        FadeManager.Instance.LoadScene("Game Clear", 2.0f);
    }
}
