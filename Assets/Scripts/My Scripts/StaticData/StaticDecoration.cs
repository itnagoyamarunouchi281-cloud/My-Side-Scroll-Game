using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDecoration : MonoBehaviour
{
    // ================================
    // �����i���ێ��p
    // ================================

    // �������������ǂ���
    public static bool isInit = false;

    // �����i�̏��i�[�z��
    // �Y�����Ԗڂ̑����i�����ꂽ���ǂ����i�[
    public static bool[] IsCreate = new bool[(int)DecorationData.DecorationType.MAX_DECO];
    
    // �X�e�[�^�X���Z�ς݂��ǂ���
    public static bool[] IsAdd = new bool[(int)DecorationData.DecorationType.MAX_DECO];

    public static void InitFlag()
    {
        for (int i = 0; i < IsCreate.Length; ++i)
        {
            IsCreate[i] = false;
            IsAdd[i] = false;
        }
    }

    // static�ȉ��Z�t���O�z����擾
    public static bool[] GetIsAdd()
    {
        return IsAdd;
    }    
    
    public static void SetIsCreate(bool[] iscreate)
    {
        for (int i = 0; i < IsCreate.Length; ++i)
        {
            IsCreate[i] = iscreate[i];
        }
    }

    public static bool[] GetIsCreate()
    {
        return IsCreate;
    }


    public static void AddDecoration()
    {
        DecorationData.DecorationType point = FindObjectOfType<DecorationData>().GetDecoType();

        // アイテムを追加する
        IsCreate[(int)point] = true;
        IsAdd[(int)point] = true;
    }
}
