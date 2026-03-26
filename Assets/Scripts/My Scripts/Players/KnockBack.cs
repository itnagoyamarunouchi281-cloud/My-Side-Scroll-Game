using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // �v���C���[�X�e�[�^�X�擾�p
    private PlayerStatus playerStatus;
    // �ڐG�����u�Ԃ̃v���C���[���W
    Vector3 prePos;
    // �ڐG�����G�̈ʒu�擾
    Vector3 enemyPos;
    // �m�b�N�o�b�N�����n�܂��Ă��邩�ۂ�
    bool isStart = false;
    // �m�b�N�o�b�N�����̃X�e�b�v(���Ԗڂ̏�����)
    int Step = 0;

    // �G�̃X�e�[�^�X�擾�p
    EnemyStatus enemyStatus;

    //CharacterController controller;
    Rigidbody rb;

    // ����s����
    float inoperableTime = 0.1f;

    // ����s�t���O
    bool isInoperable = false;

    // --- �_�ŗp ---
    // �q��Renderer�̔z��
    Renderer[] childrenRenderer;

    // ��childRenderer���L�����������̃t���O
    bool isEnabledRenderers;

    // ���_���[�W���󂯂Ă��邩�̃t���O
    bool isDamaged;

    // ���Z�b�g���鎞�ׂ̈ɃR���[�`����ێ����Ă���
    Coroutine flicker;

    // �_���[�W�̓_�ł̒����B
    float flickerDuration = 0.6f;

    // ���G����
    float invincibleTime;

    // �_���[�W�_�ł̍��v�o�ߎ���
    float flickerTotalElapsedTime;
    // �_���[�W�_�ł�Renderer�̗L���E�����؂�ւ��p�̌o�ߎ���
    float flickerElapsedTime;

    // �_���[�W�_�ł�Renderer�̗L���E�����؂�ւ��p�̃C���^�[�o��
    float flickerInterval = 0.075f;

    // �m�b�N�o�b�N����(����ȏ�̎��Ԃ��o�߂����玩���Ŏ��̃X�e�b�v)
    float KnockTime = 0.0f;

    Animator anime;

    GameObject se;

    // Start is called before the first frame update
    void Start()
    {
        // �_�Ŏ��ԂƖ��G���Ԃ����L
        invincibleTime = flickerDuration;

        playerStatus = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody>();

        childrenRenderer = GetComponentsInChildren<Renderer>();

        anime = GetComponent<Animator>();

        se = GameObject.Find("SE");
    }

    // Update is called once per frame
    void Update()
    {
        // �m�b�N�o�b�N�����X�^�[�g
        if (isStart && !anime.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
        {
            knockback();
        }
    }


    // �m�b�N�o�b�N�֐�
    public void knockback()
    {
        switch (Step)
        {
            // �v���C���[��HP�����炷
            case 0:
                anime.SetBool("isKnock", true);
                // �v���C���[���󂯂�_���[�W�i�G�̍U���� - �v���C���[�̖h��́j
                // �󂯂�_���[�W�O���傫����
                
                if (enemyStatus.GetATK() - StaticStatus.GetPlayerDEF() > 0)
                {
                    //Debug.Log(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                    playerStatus.SetMinusHp(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                }
                // �O�ȉ��Ȃ�Œ�P�_���[�W
                else
                {
                    playerStatus.SetMinusHp(1);
                }

                Step++;
                break;

            // �v���C���[������i�q�b�g�X�g�b�v�H�j
            case 1:

                // ���C���[��Invisible�ɕύX(�����蔻����Ȃ���)
                this.gameObject.layer = LayerMask.NameToLayer("Invisible");

                // ����s�t���O���I��
                isInoperable = true;

                // �����̈ʒu�ƐڐG�����I�u�W�F�N�g�̈ʒu���v�Z����
                // �����ƕ������o���Đ��K��
                Vector3 distination = new Vector3((this.transform.position.x - enemyPos.x), 0, 0).normalized;

                if (Mathf.Abs(prePos.x - this.transform.position.x) < 1)
                {
                    // �m�b�N�o�b�N
                    Knock(distination.x);

                    // �ǂɋl�܂����Ƃ��悤...�����I�Ɏ��̃X�e�b�v��
                    KnockTime += Time.deltaTime;
                    if (KnockTime > 1.0f)
                    {
                        Step++;
                    }
                }
                else
                {
                    // �ړ����������玟�̃X�e�b�v��
                    Step++;
                    anime.SetBool("isKnock", false);
                }



                break;
            // �_��
            case 2:
                if (isDamaged)
                    return;
                StartFlicker();
                Step++;
                break;

            // ���G����(2�b�A�_���[�W�d��1�b����)
            case 3:
                if (0 < invincibleTime)
                {
                    invincibleTime -= Time.deltaTime;
                    if(0 < inoperableTime)
                    {
                        inoperableTime -= Time.deltaTime;
                    }
                    else
                    {
                        // �P�b�o�����瑀��s�t���O�I�t
                        isInoperable = false;
                    }
                }
                else
                {
                    Step++;
                }
                break;

            
            case 4:
                // �m�b�N�o�b�N�����I��
                isStart = false;
                // ���������ŏ��ɖ߂�
                Step = 0;
                // ���C���[��Player�ɖ߂�
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                // ���G���ԏ�����
                invincibleTime = 2.0f;
                // ����s���ԏ�����
                inoperableTime = 0.1f;
                // �����Ŏ��̃X�e�b�v��
                KnockTime = 0.0f;
                break;
        }

    }


    void SetEnabledRenderers(bool b)
    {
        for (int i = 0; i < childrenRenderer.Length; i++)
        {
            childrenRenderer[i].enabled = b;
        }
    }

    void StartFlicker()
    {
        flicker = StartCoroutine("Flicker");
    }


    IEnumerator Flicker()
    {
        isDamaged = true;

        flickerTotalElapsedTime = 0;
        flickerElapsedTime = 0;

        while (true)
        {
            flickerTotalElapsedTime += Time.deltaTime;
            flickerElapsedTime += Time.deltaTime;

            if (flickerInterval <= flickerElapsedTime)
            {
                //��������_���[�W�_�ł̏����B
                flickerElapsedTime = 0;
                //Renderer�̗L���A�����̔��]�B
                isEnabledRenderers = !isEnabledRenderers;
                SetEnabledRenderers(isEnabledRenderers);
            }


            if (flickerDuration <= flickerTotalElapsedTime)
            {
                //��������_���[�W�_�ł̏I�����̏����B
                isDamaged = false;

                //�Ō�ɂ͕K��Renderer��L���ɂ���(�������ςȂ��ɂȂ�̂�h��)�B
                isEnabledRenderers = true;
                SetEnabledRenderers(true);

                yield break;
            }
            yield return null;
        }
    }

    //�R���[�`���̃��Z�b�g�p�B
    void ResetFlicker()
    {
        if (flicker != null)
        {
            StopCoroutine(flicker);
            flicker = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !isStart)
        {
            if (se != null)
            {
                // �U���q�b�g��
                se.GetComponent<SEManager>().PlaySE(0);
            }

            // �v���C���[���~�܂��Ă��鎞
            // �G�ƏՓ�
            isStart = true;
            // �ڐG���G�ƃv���C���[�̍��W�擾
            enemyPos = other.gameObject.transform.position;
            prePos = this.transform.position;
            // �~�܂��Ă��鎞��rigidbody�����Ă���I�u�W�F�N�g�Ɠ����蔻�������Ă���
            // ��L�̎q�I�u�W�F�N�g�ɃX�e�[�^�X��񂪂��邽�ߎq����GetComponent
            enemyStatus = other.gameObject.GetComponentInChildren<EnemyStatus>();
            
        }
    }

    private void Knock(float knockX)
    {
        // �m�b�N�o�b�N�̗͂�Rigidbody�ɉ�����
        rb.velocity = Vector2.zero; // ���݂̑��x�����Z�b�g
        rb.AddForce(new Vector2(knockX, 0), ForceMode.Impulse);
    }

    public bool GetIsInoperable()
    {
        return isInoperable;
    }
}
