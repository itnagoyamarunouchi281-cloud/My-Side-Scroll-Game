using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    // ==========================================
    // �A�C�e���f�[�^�ݒ�p
    // ==========================================
    // �A�C�e���I�u�W�F�N�g�ɃA�^�b�`
    // Inspector��Resources�t�H���_���ɂ���
    // �A�C�e���f�[�^(.asset)��ݒ肷�邾��
    // ==========================================
    public ItemData itemData;

    public Text stasText_1;
    public Text stasText_2;

    public void SetItemData()
    {
        stasText_1.text = itemData.GetItemType().ToString();
        stasText_2.text = itemData.GetItemName();
    }

    void Start()
    {
        SetItemData();
    }
}
