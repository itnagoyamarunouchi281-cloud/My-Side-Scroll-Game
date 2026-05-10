using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    // ================================
    // �G�̏����擾���ăX�e�[�^�X�ݒ�
    // ================================

    // �G�l�~�[�̏��擾�p
    EnemyInfo enemyinfo;

    // �G��MaxHP
    [SerializeField] private int maxHP;

    // �G�̌��݂�HP
    [SerializeField] private int curHP;

    // �G�̍U����
    [SerializeField] int ATK;

    [SerializeField] int DEF;

    // �G�̖��O
    [SerializeField] string enemyName;/*2024/11/20*/

    // HP�\���pUI
    [SerializeField] private GameObject HPUI;

    // HP�\���p�X���C�_�[
    private Slider hpSlider;

    
    public void Init()
    {
        // �G�̃f�[�^����X�e�[�^�X��ǂݎ��ݒ�
        enemyinfo = GetComponent<EnemyInfo>();
        curHP = maxHP = enemyinfo.enemyData.GetHp();
        ATK = enemyinfo.enemyData.GetATK();
        DEF = enemyinfo.enemyData.GetDEF();
        enemyName = enemyinfo.enemyData.GetEnemyName();/*2024/11/20*/
        HPUI.SetActive(true);
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // ���FZ�L�[��HP����
        if(Input.GetKeyUp(KeyCode.Q))
        {
            SetHp(20);            
        }
    }

    public int GetDead()
    {
        return curHP <= 0 ? 1 : 0;
    }

    public void SetHp(int hp)
    {
        if(0 < this.curHP)
            this.curHP -= hp;

        // HP�\���pUI�̃A�b�v�f�[�g
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

    /*2024/11/20*/
    public string GetEnemyName()
    {
        return enemyName;
    }

    // ���񂾂�HPUI���\���ɂ���
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxhp();
    }  
}
