using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    // ================================
    // �A�C�e���h���b�v�֘A
    // ================================
    // �G�ɃA�^�b�`
    // �G�̎�ނɉ����ăA�C�e�����h���b�v������
    EnemyInfo enemyinfo;

    // �v���C���[�̃��b�N�l���擾����ϐ�
    public float playerLuck;

    // �A�C�e���̏��i�h���b�v���j���擾
    private float DropRate;

    // �����i�[�p
    private int random;

    // �h���b�v����A�C�e���I�u�W�F�N�g
    // �G�P�̂ŕ���������\�����l��
    public List<GameObject> ObjLists = new List<GameObject>();

    public void Init()
    {
        // EnemyInfo�R���|�[�l���g�擾�p
        enemyinfo = GetComponent<EnemyInfo>();

        // �G���A�C�e�����h���b�v����m��
        DropRate = enemyinfo.enemyData.GetDroprate();
    }

    // Update is called once per frame
    void Update()
    {
        // �f�o�b�O�\���F�G�̎��
        //Debug.Log(enemyinfo.enemyData.GetEnemyType());
    }

    public void ItemDrop()
    {
        // ObjLists�ɐݒ肳��Ă���A�C�e�����h���b�v
        for (int i = 0; i < ObjLists.Count; ++i)
        {
            // ��������(1~100)
            random = Random.Range(1, 100);
            // �m���Ńh���b�v
            // �E�ӂ̒l(�A�C�e���h���b�v�l)��100�ȏ�ɂȂ�Ίm��h���b�v
            if (random <= (playerLuck * DropRate) / 1000)
            {
                GameObject itemObj = Instantiate(ObjLists[i], 
                    transform.position + new Vector3(i,0,0), 
                    Quaternion.identity);
                
            }
        }
    }
}
