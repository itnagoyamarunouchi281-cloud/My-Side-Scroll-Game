using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Move : MonoBehaviour
{
    public enum Enemy02Mode
    {
        WALK,       // ����
        BACK,       // �߂�i�����ʒu�ցj
        CHASE,      // �ǐ�
        ATTACK,     // �U��
        DIE,        // �|���
        KNOCK,

        PLAYER_DIE, // �v���C���[���|�ꂽ�Ƃ�

        MAX
    }

    [SerializeField] private Enemy02Mode curMode;

    [SerializeField] private Enemy02Mode initialMode = Enemy02Mode.WALK;

    [SerializeField] private Enemy02Mode preMode;

    private Enemy enemy;
    private EnemyStatus status;

    [SerializeField] private float walkRange = 2.0f;
    [SerializeField] private float visualRange = 5.0f;
    [SerializeField] private float moveY = 0.9f;

    private GameObject playerObj;
    private Player player;

    private Animator animator;
    private Rigidbody rb;

    private Vector3 initPos;
    private Transform thistrans;
    private bool isStart = false;
    private bool isDead = false;

    private GameObject scissors;
    private Vector3 pos;
    private int Step;
    private float dir = 1; // ����
    private Vector3 newDir; // BackMode�p�̕���

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        rb = GetComponent<Rigidbody>();

        // �A�j���[�V�����R���g���[���[
        animator = GetComponent<Animator>();

        // �������[�h�擾
        curMode = Enemy02Mode.WALK;

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

        thistrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // ���W�擾
        pos = thistrans.position;

        // �̗͂O�ɂȂ����烂�[�h�ύX
        if (status.GetHp() <= 0)
        {
            curMode = Enemy02Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        if(playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy02Mode.WALK)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            curMode = Enemy02Mode.PLAYER_DIE;
        }

        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // �t���O�I��
            isStart = true;
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy02Mode.KNOCK;
        }

        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy02Mode.WALK:
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
                ObakeSearch(dir);

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy02Mode.BACK:
                // �����ʒu�֖߂�������擾
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                ObakeSearch(newDir.x);
                
                // ������ێ�������
                dir = newDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy02Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime * moveY;
                }
                break;

            case Enemy02Mode.CHASE:
                // ����������
                newDir = new Vector3(
                    (player.transform.position.x - thistrans.position.x),
                    (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // �ǐՃA�j���J�n(����������)
                animator.SetBool("isChase", true);
                
                // �U���ֈڂ�
                if(Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
                {
                    thistrans.position = new Vector3(player.transform.position.x,0,0);
                    curMode = Enemy02Mode.ATTACK;
                }


                // ����������
                // �v���C���[�����F������艓���ɍs�����ːi���Ă���G�̌��ɍs�����Ƃ�
                if (thistrans.position.x +  visualRange < player.transform.position.x ||
                    thistrans.position.x - visualRange > player.transform.position.x)
                {
                    Debug.Log("�����؂���");
                    // Attack���I��
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);
                    // �����ʒu�֖߂郂�[�h��
                    curMode = Enemy02Mode.BACK;
                }


                // Chase�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
                {
                    rb.useGravity = false;
                    pos.x += newDir.x * Time.deltaTime * 2.0f;
                    if (pos.y < initPos.y + 10.0f)
                    {
                        pos.y += newDir.y * Time.deltaTime * moveY;
                    }
                }
                break;

            case Enemy02Mode.ATTACK:
                newDir = new Vector3(0, player.transform.position.y - thistrans.position.y, 0);
                // �U���A�j��
                animator.SetBool("isAttack", true);
                // �U���A�j���ɍ��킹�č~��
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
                    {
                        pos.y += newDir.y * Time.deltaTime * moveY;
                    }
                }

                // �U���A�j�����I�������AttackEnd�Ɏ����J��
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
                {
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);

                    // �͈͊O
                    if (thistrans.position.x + visualRange < player.transform.position.x ||
                        thistrans.position.x - visualRange > player.transform.position.x)
                    {
                        curMode = Enemy02Mode.WALK;
                    }
                    else
                    {
                        curMode = Enemy02Mode.CHASE;
                    }
                }

                break;

            case Enemy02Mode.DIE:
                // �f�o�b�O�p
                if (Input.GetKeyDown(KeyCode.Z))
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

            case Enemy02Mode.PLAYER_DIE:
                // �����ʒu�֖߂�������擾
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // ������ێ�������
                dir = newDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy02Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime * moveY;
                }
                break;

            case Enemy02Mode.KNOCK:
                if (isStart)
                {
                    animator.SetBool("isKnock", true);
                    KnockBack();
                }
                break;
        }


        switch (curMode)
        {
            case Enemy02Mode.WALK:
                WalkMode();
                break;
            case Enemy02Mode.BACK:
                BackMode();
                break;
            case Enemy02Mode.CHASE:
                ChaseMode();
                break;
            case Enemy02Mode.ATTACK:
                AttackMode();
                break;
            case Enemy02Mode.DIE:
                DieMode();
                break;
            case Enemy02Mode.PLAYER_DIE:
                PlayerDieMode();
                break;
            case Enemy02Mode.KNOCK:
                if (isStart) { KnockBack(); }
                break;
        }

        // ���W�X�V
        thistrans.position = pos;
    }

    private void WalkMode()
    {
        if (thistrans.position.x > initPos.x + walkRange)
        {
            dir = -1;
        }
        if (thistrans.position.x < initPos.x - walkRange)
        {
            dir = 1;
        }
        ObakeSearch(dir);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            pos.x += dir * Time.deltaTime;
        }
    }

    private void BackMode()
    {
        // �����ʒu�֖߂�������擾
        newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        // �v���C���[�����F�͈͂ɂ��邩
        ObakeSearch(newDir.x);

        // ������ێ�������
        dir = newDir.x;

        // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
        if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
        {
            // �������[�h��
            curMode = Enemy02Mode.WALK;
        }

        // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            pos.x += newDir.x * Time.deltaTime;
            pos.y += newDir.y * Time.deltaTime * moveY;
        }
    }

    private void ChaseMode()
    {
        // ����������
        newDir = new Vector3(
            (player.transform.position.x - thistrans.position.x),
            (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        // �ǐՃA�j���J�n(����������)
        animator.SetBool("isChase", true);

        // �U���ֈڂ�
        if (Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
        {
            thistrans.position = new Vector3(player.transform.position.x, 0, 0);
            curMode = Enemy02Mode.ATTACK;
        }


        // ����������
        // �v���C���[�����F������艓���ɍs�����ːi���Ă���G�̌��ɍs�����Ƃ�
        if (thistrans.position.x + visualRange < player.transform.position.x ||
            thistrans.position.x - visualRange > player.transform.position.x)
        {
            Debug.Log("�����؂���");
            // Attack���I��
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            // �����ʒu�֖߂郂�[�h��
            curMode = Enemy02Mode.BACK;
        }


        // Chase�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            rb.useGravity = false;
            pos.x += newDir.x * Time.deltaTime * 2.0f;
            if (pos.y < initPos.y + 10.0f)
            {
                pos.y += newDir.y * Time.deltaTime * moveY;
            }
        }
    }

    private void AttackMode()
    {
        newDir = new Vector3(0, player.transform.position.y - thistrans.position.y, 0);
        // �U���A�j��
        animator.SetBool("isAttack", true);
        // �U���A�j���ɍ��킹�č~��
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
            {
                pos.y += newDir.y * Time.deltaTime * moveY;
            }
        }

        // �U���A�j�����I�������AttackEnd�Ɏ����J��
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);

            // �͈͊O
            if (thistrans.position.x + visualRange < player.transform.position.x ||
                thistrans.position.x - visualRange > player.transform.position.x)
            {
                curMode = Enemy02Mode.WALK;
            }
            else
            {
                curMode = Enemy02Mode.CHASE;
            }
        }
    }

    private void DieMode()
    {
        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enemy != null)
            {
                enemy.SetIsDead(true);
            }

            if (!isDead)
            {
                StaticEnemy.IsUpdate = true;
                isDead = true;
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
    }

    private void PlayerDieMode()
    {
        // �����ʒu�֖߂�������擾
        newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        // ������ێ�������
        dir = newDir.x;

        // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
        if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
        {
            // �������[�h��
            curMode = Enemy02Mode.WALK;
        }

        // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            pos.x += newDir.x * Time.deltaTime;
            pos.y += newDir.y * Time.deltaTime * moveY;
        }
    }

    public void ObakeSearch(float Dir)
    {
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // �v���C���[�𔭌�������
            // �E�����Ă���Ƃ�
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // �ːi(�U��)���[�h��
                curMode = Enemy02Mode.CHASE;
            }
            // �������Ă���Ƃ�
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy02Mode.CHASE;
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

                // ���������ŏ��ɖ߂�
                Step = 0;
                // �m�b�N�o�b�N�O�̃��[�h�ɖ߂�
                curMode = preMode;
                // �m�b�N�o�b�N�A�j���I��
                animator.SetBool("isKnock", false);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.isKinematic = true;
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", true);
            ////�����ʒu�֖߂郂�[�h��
            ///�������[�h��
            curMode = Enemy02Mode.WALK;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.isKinematic = false;
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", false);
            isStart = false;
        }
    }
}
