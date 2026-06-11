using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    EnemyInfo enemyinfo;

    [SerializeField] private int maxHP;

    [SerializeField] private int curHP;

    [SerializeField] int ATK;

    [SerializeField] int DEF;

    [SerializeField] string enemyName;

    [SerializeField] private GameObject HPUI;

    private Slider hpSlider;

    
    public void Init()
    {
        enemyinfo = GetComponent<EnemyInfo>();
        curHP = maxHP = enemyinfo.enemyData.GetHp();
        ATK = enemyinfo.enemyData.GetATK();
        DEF = enemyinfo.enemyData.GetDEF();
        enemyName = enemyinfo.enemyData.GetEnemyName();/*2024/11/20*/
        HPUI.SetActive(true);
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1.0f;
    }

    public int GetDead()
    {
        return curHP <= 0 ? 1 : 0;
    }

    public void SetHp(int hp)
    {
        if(0 < this.curHP)
            this.curHP -= hp;

        UpdateHPValue();
    }

    public int GetHp()
    {
        return curHP;
    }

    public int GetMaxhp()
    {
        return maxHP;
    }

    public int GetATK()
    {
        return ATK;
    }

    public int GetDEF()
    {
        return DEF;
    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxhp();
    }  
}
