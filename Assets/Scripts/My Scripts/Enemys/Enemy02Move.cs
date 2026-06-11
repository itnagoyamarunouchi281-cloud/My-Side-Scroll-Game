using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Move : MonoBehaviour
{
    public enum Enemy02Mode
    {
        WALK,
        BACK,
        CHASE,
        ATTACK,
        DIE,
        KNOCK,

        PLAYER_DIE,

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
    private float dir = 1;
    private Vector3 newDir;

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        curMode = Enemy02Mode.WALK;

        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        scissors = GameObject.Find("scissors1");
        Step = 0;

        thistrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        pos = thistrans.position;

        if (status.GetHp() <= 0)
        {
            curMode = Enemy02Mode.DIE;
        }

        if(playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy02Mode.WALK)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            curMode = Enemy02Mode.PLAYER_DIE;
        }

        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            isStart = true;
            preMode = curMode;
            curMode = Enemy02Mode.KNOCK;
        }

        switch (curMode)
        {
            case Enemy02Mode.WALK:

                if (thistrans.position.x > initPos.x + walkRange)
                {
                    dir = -1;
                }

                if (thistrans.position.x < initPos.x - walkRange)
                {
                    dir = 1;
                }
                //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                ObakeSearch(dir);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;

            case Enemy02Mode.BACK:
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                ObakeSearch(newDir.x);
                
                dir = newDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy02Mode.WALK;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime * moveY;
                }
                break;

            case Enemy02Mode.CHASE:
                newDir = new Vector3(
                    (player.transform.position.x - thistrans.position.x),
                    (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                animator.SetBool("isChase", true);
                
                if(Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
                {
                    thistrans.position = new Vector3(player.transform.position.x,0,0);
                    curMode = Enemy02Mode.ATTACK;
                }

                if (thistrans.position.x +  visualRange < player.transform.position.x ||
                    thistrans.position.x - visualRange > player.transform.position.x)
                {
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);
                    curMode = Enemy02Mode.BACK;
                }

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
                animator.SetBool("isAttack", true);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
                    {
                        pos.y += newDir.y * Time.deltaTime * moveY;
                    }
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
                {
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);

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

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }
                }

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
            
                newDir = new Vector3(initPos.x - thistrans.position.x, initPos.y - thistrans.position.y, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                dir = newDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy02Mode.WALK;
                }

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
                if (isStart)
                {
                    animator.SetBool("isKnock", true);
                    KnockBack();
                }
                break;
        }

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
        newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        ObakeSearch(newDir.x);

        dir = newDir.x;

        if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
        {
            curMode = Enemy02Mode.WALK;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            pos.x += newDir.x * Time.deltaTime;
            pos.y += newDir.y * Time.deltaTime * moveY;
        }
    }

    private void ChaseMode()
    {
        newDir = new Vector3(
            (player.transform.position.x - thistrans.position.x),
            (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        animator.SetBool("isChase", true);

        if (Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
        {
            thistrans.position = new Vector3(player.transform.position.x, 0, 0);
            curMode = Enemy02Mode.ATTACK;
        }

        if (thistrans.position.x + visualRange < player.transform.position.x ||
            thistrans.position.x - visualRange > player.transform.position.x)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            curMode = Enemy02Mode.BACK;
        }

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
        animator.SetBool("isAttack", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
            {
                pos.y += newDir.y * Time.deltaTime * moveY;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);

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
        newDir = new Vector3(initPos.x - thistrans.position.x, initPos.y - thistrans.position.y, 0).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

        dir = newDir.x;

        if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
        {
            curMode = Enemy02Mode.WALK;
        }

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
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                curMode = Enemy02Mode.CHASE;
            }

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
        Vector3 distination = new Vector3(this.transform.position.x - player.transform.position.x, 0, 0).normalized;

        if(Step == 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
            {
                pos.x += distination.x * Time.deltaTime;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Step++;
            }
        }
        else if(Step == 1)
        {
            Step = 0;
            curMode = Enemy02Mode.BACK;
            animator.SetBool("isKnock", false);
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
            {
                pos.x += distination.x * Time.deltaTime;
                pos.y += distination.y * Time.deltaTime;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Step++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.isKinematic = true;
        if (collision.gameObject.tag == "Player")
        {
            isStart = true;
            animator.SetBool("isCollide", true);
            curMode = Enemy02Mode.KNOCK;
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
