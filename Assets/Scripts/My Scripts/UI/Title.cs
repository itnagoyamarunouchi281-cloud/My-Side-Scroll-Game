using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // �A�v���P�[�V�������I��
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SceneChange(string sceneName)
    {
        FadeManager.Instance.LoadScene(sceneName, 1.0f);
    }
}
