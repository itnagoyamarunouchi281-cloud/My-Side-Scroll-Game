using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticClear : MonoBehaviour
{
    // ================================
    // �X�e�[�W�N���A���Ǘ�
    // ================================
    // --- �z�肷��g���� ---
    // �X�e�[�W�I����StageNo�ɗV�ԃX�e�[�W�ԍ���ݒ�
    // �X�e�[�W�N���A��
    // AddClearNum��ClearNum�����Z
    // �I���V�[����GetClearNum�Ŕ��f
    // �N���A�����X�e�[�W�� + 1�X�e�[�W���
    // ���Q�[���J�n���͂P�X�e�[�W������

    public List<Button> LevelButtonList;
    public int levelNum = 0;

    // �V��ł���X�e�[�W�ԍ�
    public static int StageNo;

    // �N���A�X�e�[�W��
    public static int ClearNum = 0;


    public static int GetClearNum()
    {
        return ClearNum;
    }
    
    
    // �N���A�����Z
    public static void AddClearNum()
    {
        // �V��ł���X�e�[�W�ԍ����N���A���Ă���X�e�[�W�����傫���Ƃ��������Z
        // ���N���A�̃X�e�[�W���N���A�������̂݃X�e�[�W���
        if (StageNo > ClearNum)
        {
            ClearNum++;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        LevelButtonList[ClearNum].interactable = true;
        ClearNum++;

        if(ClearNum > levelNum)
        {
            FadeManager.Instance.LoadScene("ClearScene", 1.0f);
        }
    }
}
