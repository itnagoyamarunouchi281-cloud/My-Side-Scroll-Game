using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChoiceLevelNo : MonoBehaviour
{
    public List<Button> LevelButtonList;

    [SerializeField] private StaticClear staticClear;
    [SerializeField] private Button button_ranking;

    bool isTotalClear = false;
    int i;

    void Start()
    {
        button_ranking.interactable = isTotalClear;

        staticClear.LevelNo(LevelButtonList.Count);

        int clearCount = staticClear.GetClearNum();

        while (i < LevelButtonList.Count)
        {
            // Level1 is always unlocked.
            // Levels 2..N unlock one by one as clearCount increases.
            LevelButtonList[i].interactable = (i == 0 || i <= clearCount);

            i++;
        }

        if(i <= clearCount)
        {
            isTotalClear = true;
            button_ranking.interactable = isTotalClear;
        }
    }
}
