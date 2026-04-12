using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Move : MonoBehaviour
{
    public enum Enemy01Mode
    {
        WALK,
        BACK,
        RUSH,
        DIE,
        KNOCK,

        PLAYER_DIE,

        MAX
    }

    [SerializeField] private Enemy01Mode curMode;

    [SerializeField] private Enemy01Mode initialMode = Enemy01Mode.WALK;

    [SerializeField] private Enemy01Mode preMode;

    [Header("Movement Parameters")]
    [SerializeField] private float walkRange = 2.0f;
    [SerializeField] private float visualRange = 5.0f;
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float rushSpeed = 2.0f;

    private Vector3 initPos;
    private GameObject playerObj;
    private Player player;
    private Animator animator;
    private AnimatorStateInfo animeInfo;
    private Transform thistrans;
    private Rigidbody rb;
    private GameObject scissors;
    private Vector3 pos;
    private float KnockTime = 0.0f;
    private int Step;
    private bool isStart = false;
    private bool isDead = false;
    private Enemy enemy;
    private EnemyStatus status;
    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;

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

        if(status.GetHp() <= 0)
        {
            curMode = Enemy01Mode.DIE;
        }

        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy01Mode.WALK)
        {
            animator.SetBool("isAttack", false);
            curMode = Enemy01Mode.PLAYER_DIE;
        }
        
        if(scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            isStart = true;
            preMode = curMode;
            curMode = Enemy01Mode.KNOCK;
        }

        switch (curMode)
        {
            case Enemy01Mode.WALK:
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


            case Enemy01Mode.BACK:

                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                Search(BackDir.x);

                dir = BackDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy01Mode.WALK;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy01Mode.RUSH:

                animator.SetBool("isAttack", true);

                if ((dir == 1 && (thistrans.position.x + dir * visualRange < player.transform.position.x ||
                    player.transform.position.x < thistrans.position.x)) ||
                    (dir == -1 && (thistrans.position.x + dir * visualRange > player.transform.position.x ||
                    player.transform.position.x > thistrans.position.x)))
                {
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isCollide", false);
                    curMode = Enemy01Mode.BACK;
                    break;
                }
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rush"))
                {
                    pos.x += dir * Time.deltaTime * 2.0f;
                }
                break;

            case Enemy01Mode.DIE:

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

                break;

            case Enemy01Mode.PLAYER_DIE:
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                dir = BackDir.x;

                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    curMode = Enemy01Mode.WALK;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy01Mode.KNOCK:
                if(isStart)
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
                curMode = Enemy01Mode.RUSH;
            }

            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy01Mode.RUSH;
            }
        }
    }

    private void KnockBack()
    {
        
        switch (Step)
        {
            case 0:

                Vector3 distination = new Vector3((this.transform.position.x - player.transform.position.x), 0, 0).normalized;
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
                {
                    pos.x += distination.x * Time.deltaTime;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Step++;
                }

                break;
            
            case 1:
                Step = 0;
                curMode = preMode;
                animator.SetBool("isKnock", false);
                
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;

        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", true);
            curMode = Enemy01Mode.BACK;
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
