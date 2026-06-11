using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03Move : MonoBehaviour
{
    public enum Enemy03Mode
    {
        WALK,
        BACK,
        RUSH,
        DIE,
        KNOCK,

        PLAYER_DIE,

        MAX
    }

    [SerializeField] private Enemy03Mode curMode;

    [SerializeField] private Enemy03Mode initialMode = Enemy03Mode.WALK;

    [SerializeField] private Enemy03Mode preMode;

    Enemy enemy;
    EnemyStatus status;

    private Vector3 initPos;

    private float walkRange = 2.0f;

    private float visualRange = 5.0f;

    private GameObject playerObj;

    Player player;

    private Animator animator;
    private AnimatorStateInfo animeInfo;

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
        animator = GetComponent<Animator>();

        curMode = initialMode;
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        scissors = GameObject.Find("scissors1");
        Step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        thistrans = this.transform;
        pos = thistrans.position;

        if (status.GetHp() <= 0)
        {
            curMode = Enemy03Mode.DIE;
        }

        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy03Mode.WALK)
        {
            animator.SetBool("isAttack", false);
            curMode = Enemy03Mode.PLAYER_DIE;
        }

        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            preMode = curMode;
            curMode = Enemy03Mode.KNOCK;
            isStart = true;
        }

        switch (curMode)
        {
            case Enemy03Mode.WALK:

                if (thistrans.position.x > initPos.x + walkRange)
                {
                    dir = -1;
                }

                if (thistrans.position.x < initPos.x - walkRange)
                {
                    dir = 1;
                }
                //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                Search(dir);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;

            case Enemy03Mode.BACK:
                
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                Search(BackDir.x);

                dir = BackDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy03Mode.WALK;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy03Mode.RUSH:
                animator.SetBool("isAttack", true);

                if ((dir == 1 && (thistrans.position.x + dir * visualRange < player.transform.position.x ||
                    player.transform.position.x < thistrans.position.x)) ||
                    (dir == -1 && (thistrans.position.x + dir * visualRange > player.transform.position.x ||
                    player.transform.position.x > thistrans.position.x)))
                {
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isCollide", false);
                    curMode = Enemy03Mode.BACK;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rush"))
                {
                    pos.x += dir * Time.deltaTime * 2.0f;
                }
                break;

            case Enemy03Mode.DIE:

                if (Input.GetKeyDown(KeyCode.Return))
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

            case Enemy03Mode.PLAYER_DIE:

                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                dir = BackDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy03Mode.WALK;
                }

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

        thistrans.position = pos;
    }

    public void Search(float Dir)
    {
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                curMode = Enemy03Mode.RUSH;
            }

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
            isStart = false;
            Step = 0;
            curMode = preMode;
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

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        if (collision.gameObject.tag == "Player")
        {   
            isStart = true;
            animator.SetBool("isCollide", true);
            curMode = Enemy03Mode.KNOCK;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.isKinematic = false;
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", false);
            isStart = false;
        }
    }
}
