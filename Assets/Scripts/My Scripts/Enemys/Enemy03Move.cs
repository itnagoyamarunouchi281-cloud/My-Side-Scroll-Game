using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03Move : MonoBehaviour
{
    public enum Enemy03Mode
    {
        WALK,       // ����
        BACK,       // �߂�i�����ʒu�ցj
        RUSH,       // �ːi
        DIE,        // �|���
        KNOCK,

        PLAYER_DIE, // �v���C���[���|�ꂽ��

        MAX
    }

    /// ���݂̃��[�h
    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy03Mode curMode;

    [SerializeField] private Enemy03Mode initialMode = Enemy03Mode.WALK;

    [SerializeField] private Enemy03Mode preMode;

    Enemy enemy;
    EnemyStatus status;

    // �����ʒu�擾�p
    private Vector3 initPos;

    // �����͈�(�Q�[���J�n���̃X�|�[���ʒu���N�_)
    private float walkRange = 2.0f;

    // �v���C���[�����F����͈�
    private float visualRange = 5.0f;

    // private��Player�擾
    private GameObject playerObj;

    // �v���C���[�̍��W�擾�p
    Player player;

    private Animator animator;
    private AnimatorStateInfo animeInfo;

    // �����Ă������
    [SerializeField] private float dir;

    Transform thistrans;

    [SerializeField] private Vector3 BackDir;


    Rigidbody rb;

    GameObject scissors;
    Vector3 pos;
    float KnockTime = 0.0f;
    int Step;
    bool isStart = false;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        rb = GetComponent<Rigidbody>();

        // �A�j���[�V�����R���g���[���[
        animator = GetComponent<Animator>();

        // �������[�h�擾
        curMode = initialMode;

        // �����ʒu�擾
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        // �͂���
        scissors = GameObject.Find("scissors1");

        // �G�m�b�N�o�b�N�����p
        Step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ���W�擾
        thistrans = this.transform;
        pos = thistrans.position;

        // �̗͂O�ɂȂ����烂�[�h�ύX
        if (status.GetHp() <= 0)
        {
            curMode = Enemy03Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy03Mode.WALK)
        {
            animator.SetBool("isAttack", false);
            curMode = Enemy03Mode.PLAYER_DIE;
        }

        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy03Mode.KNOCK;
            // �t���O�I��
            isStart = true;

        }

        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy03Mode.WALK:
                // �E�[�ɍs�����獶�֕����]��
                if (thistrans.position.x > initPos.x + walkRange)
                {
                    dir = -1;
                }
                // ���[�ɍs������E�֕����]��
                if (thistrans.position.x < initPos.x - walkRange)
                {
                    dir = 1;
                }
                //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                Search(dir);

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy03Mode.BACK:
                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                Search(BackDir.x);

                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy03Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy03Mode.RUSH:
                // �U���A�j���J�n(�W�����v���Z�b�g���ːi)
                animator.SetBool("isAttack", true);


                // ����������
                // �v���C���[�����F������艓���ɍs�����ːi���Ă���G�̌��ɍs�����Ƃ�
                if ((dir == 1 && (thistrans.position.x + dir * visualRange < player.transform.position.x ||
                    player.transform.position.x < thistrans.position.x)) ||
                    (dir == -1 && (thistrans.position.x + dir * visualRange > player.transform.position.x ||
                    player.transform.position.x > thistrans.position.x)))
                {
                    Debug.Log("�����؂���");
                    // Attack���I��
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isCollide", false);
                    // �����ʒu�֖߂郂�[�h��
                    curMode = Enemy03Mode.BACK;

                }

                // Rush�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rush"))
                {
                    pos.x += dir * Time.deltaTime * 2.0f;
                }
                break;

            case Enemy03Mode.DIE:
                // �f�o�b�O�p
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }
                }

                // �|��郂�[�V����
                animator.SetBool("isDie", true);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
                {
                    enemy.SetIsDead(true);

                    if (!isDead)
                    {
                        StaticEnemy.IsUpdate = true;
                        isDead = true;
                    }
                }

                break;

            case Enemy03Mode.PLAYER_DIE:
                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy03Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy03Mode.KNOCK:
                if (isStart)
                {
                    animator.SetBool("isKnock", true);
                    KnockBack();
                }
                break;
        }

        // ���W�X�V
        thistrans.position = pos;

    }




    public void Search(float Dir)
    {
        // �v���C���[���|��ĂȂ���ΒT��
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // �v���C���[�𔭌�������
            // �E�����Ă���Ƃ�
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // �ːi(�U��)���[�h��
                curMode = Enemy03Mode.RUSH;
            }
            // �������Ă���Ƃ�
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy03Mode.RUSH;
            }
        }
    }

    private void KnockBack()
    {
        switch (Step)
        {
            // �v���C���[������i�q�b�g�X�g�b�v�H�j
            case 0:
                // �����̈ʒu�ƐڐG�����I�u�W�F�N�g�̈ʒu���v�Z����
                // �����ƕ������o���Đ��K��
                Vector3 distination = new Vector3((this.transform.position.x - player.transform.position.x), 0, 0).normalized;

                // �m�b�N�o�b�N�A�j�����Đ�����Ă����
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
                {
                    // �m�b�N�o�b�N
                    pos.x += distination.x * Time.deltaTime;
                }
                // �Đ�����Ă���A�j�����I�������
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    // �ړ����������玟�̃X�e�b�v��
                    Step++;
                }
                break;

            case 1:
                // �m�b�N�o�b�N�����I��
                isStart = false;
                // ���������ŏ��ɖ߂�
                Step = 0;
                // �m�b�N�o�b�N�O�̃��[�h�ɖ߂�
                curMode = preMode;
                // �m�b�N�o�b�N�A�j���I��
                animator.SetBool("isKnock", false);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        // �v���C���[
        if (collision.gameObject.tag == "Player")
        {
            // �Ԃ����������
            animator.SetBool("isCollide", true);
            ////�����ʒu�֖߂郂�[�h��
            curMode = Enemy03Mode.BACK;
            Debug.Log("�Ԃ�����");
        }
        //curMode = EnemyMode.WALK;
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.isKinematic = false;
        // �v���C���[
        if (collision.gameObject.tag == "Player")
        {
            // �Ԃ����ė��ꂽ��
            animator.SetBool("isCollide", false);

        }
    }
}
