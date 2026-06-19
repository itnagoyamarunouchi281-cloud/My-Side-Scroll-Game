using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticClear : MonoBehaviour
{
    public List<Button> LevelButtonList;
    
    public static int ClearNum = 0;
    private static int StageNo = 0;


    public int GetClearNum()
    {
        return ClearNum;
    }

    public void LevelNo(int no)
    {
        StageNo = no;
    }
    
    public static void AddClearNum()
    {
        if (StageNo > ClearNum)
        {
            ClearNum++;
        }
    }

    public static bool IsAllCleared()
    {
        return StageNo > 0 && ClearNum >= StageNo;
    }

    void Start()
    {
        if(IsAllCleared())
        {
            FadeManager.Instance.LoadScene("ClearScene", 1.0f);
        }
    }
}
