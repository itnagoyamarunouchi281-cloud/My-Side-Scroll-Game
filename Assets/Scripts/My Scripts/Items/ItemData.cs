using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ItemData",menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    public enum Type
    {
        アングラ,
        文化民族,
        エレメント,
        ファンタジー,
        ゴシック,
        ホラー,
        海賊,
        カジュアル,
        虫動物,
        謎系,
        ノーマル,
        SF,
        神話,
        スチームパンク,
        和風,

        MAX_ITEM
    }

    [SerializeField] private Type itemType;
    [SerializeField] private Sprite icon;
    [SerializeField] private string itemName;
    [SerializeField] private int Num;
   

    public Type GetItemType()
    {
        return itemType;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemNum()
    {
        return Num;
    }
}
