using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChoiceLevelNo : MonoBehaviour
{
    public List<Button> LevelButtonList;

    [SerializeField] private StaticClear staticClear;

    private int clearNum = 0;

    void Start()
    {
        staticClear.LevelNo(LevelButtonList.Count);

        for(int i = 1; i < LevelButtonList.Count - 1; i++)
        {
            LevelButtonList[i].interactable = false;
        }

        while(clearNum < LevelButtonList.Count)
        {
            if(clearNum <= staticClear.GetClearNum())
            {
                LevelButtonList[clearNum].interactable = true;
            }

            clearNum++;
        }
    }
}
