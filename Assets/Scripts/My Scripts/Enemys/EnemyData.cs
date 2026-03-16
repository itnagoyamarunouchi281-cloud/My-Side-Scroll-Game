using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]
public class EnemyData : ScriptableObject
{
    // ================================
    // エネミーの種類
    // ================================
    // ↓ここに増やしていくだけでOK
    // ================================
    public enum EnemyType
    {
        マ一号,
        マ二号,
        マ三号,
        マ四号,
        マ五号,
        マ六号,
        マ七号,
        マ八号,
        マ九号,
        マ十号,
        ト一号,
        ト二号,
        ト三号,
        ト四号,
        ト五号,
        ト六号,
        ト七号,
        ト八号,
        ト九号,
        ト十号,


        MAX_ENEMY
    }

    // 敵の種類
    [SerializeField] private EnemyType enemyType;
    // 敵の名前
    [SerializeField] private string enemyName;
    // 敵が落とすアイテム
    [SerializeField] private ItemData.Type itemtype;

    // アイテムのドロップ率
    [SerializeField] private int Droprate;

    // 敵のHP
    [SerializeField] private int maxHp;
    // 敵の攻撃力
    [SerializeField] private int ATK;
    // 敵の防御力
    [SerializeField] private int DEF;

    // 敵の種類取得
    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    // 敵の名前取得
    public string GetEnemyName()
    {
        return enemyName;
    }

    public ItemData.Type GetItemType()
    {
        return itemtype;
    }

    public int GetHp()
    {
        return maxHp;
    }

    public int GetATK()
    {
        return ATK;
    }

    public int GetDEF()
    {
        return DEF;
    }

    // アイテムのドロップ率取得
    public int GetDroprate()
    {
        return Droprate;
    }
}
