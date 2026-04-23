using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxJumpHeight = 3.2f;
    public float maxJumpDistance = 8.0f;
    public float gravity;
    public GameObject charaobj;
    public GameObject camobj;

    [SerializeField] private float rayLength = 1f;

    [SerializeField] private float rayOffset;

    [SerializeField] private LayerMask layerMask = default;

    private float x;

    private Vector3 moveDirection = Vector3.zero;

    private ItemInfo iteminfo;
    private MyItem myitem;

    private CharacterController controller;
    private KnockBack knock;
    
    [SerializeField] private Animator anime;

    private bool isJump;

    private bool isGround;
    private GameObject scissors1;
    private float LeaveTime = 0.0f;
    private int WalkTimer = 0;

    // Use this for initialization
    void Start()
    {
        myitem = GetComponent<MyItem>();

        controller = GetComponent<CharacterController>();
        knock = GetComponent<KnockBack>();

        scissors1 = GameObject.Find("scissors1");
        isJump = false;
    }

    void Update()
    {
        anime.SetBool("isWalk", false);

        Vector3 effectpos = this.gameObject.transform.position;

        effectpos.x = this.gameObject.transform.position.x - 0.6f;
        effectpos.y = this.gameObject.transform.position.y - 1.1f;

        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Anim_Idle"))
        {
            LeaveTime += Time.deltaTime;
            if (LeaveTime > 5.0f)
            {
                anime.SetBool("isLeave", true);
            }
        }

        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Doya"))
        {
            Vector3 newDir =
                Vector3.RotateTowards(
                    transform.forward, new Vector3(0, 0, -1),
                    4.5f * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDir);
            LeaveTime = 0.0f;
            anime.SetBool("isLeave", false);
        }

        AttackMotion();

        if (scissors1.GetComponent<AttackContoroll>().GethitFlg())
        {
            moveDirection.y = 1;
        }

        x = Input.GetAxis("Horizontal");

        if (!controller.isGrounded)
        {
            if (CheckGrounded())
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }
        
        if (controller.isGrounded && isGround)
        {
            anime.SetBool("isJump", false);
            isJump = false;

            moveDirection = new Vector3(0, 0, x);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            //moveDirection.x *= Vec;

            if (Input.GetKeyDown(KeyCode.Space) &&
                !anime.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anime.SetBool("isJump", true);
                anime.SetFloat("animSpeed", 2.0f);
                isJump = true;
                moveDirection.y = maxJumpHeight * 0.8f;
            }

            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
                anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.27 &&
               isJump)
            {
                isJump = false;
                anime.SetFloat("animSpeed", 0.5f);
                moveDirection.y = maxJumpHeight * 0.8f;
            }

            if (!knock.GetIsInoperable())
            {
                if(x == 0)
                {
                    LeaveTime += Time.deltaTime;
                }
                else if (x > 0)
                {
                    LeaveTime = 0.0f;
                    anime.SetBool("isWalk", true);
                    moveDirection.x = Input.GetAxis("Horizontal") * speed * 0.8f;
                    gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                    charaobj.transform.localScale = new Vector3(1, 1, 1);
                    WalkTimer++;
                }

                else if (x < 0)
                {
                    LeaveTime = 0.0f;
                    anime.SetBool("isWalk", true);
                    moveDirection.x = Input.GetAxis("Horizontal") * speed * 0.8f;
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    charaobj.transform.localScale = new Vector3(-1, 1, 1);
                    WalkTimer++;
                }
            }
        }
        else
        {
            moveDirection.x = Input.GetAxis("Horizontal") * (speed / 2);
        }

        if (WalkTimer == 15)
        {
            WalkTimer = 0;
        }

        Vector3 pos = transform.position;
        //pos.x = 0.0f;
        transform.position = pos;

        moveDirection.y -= gravity * Time.deltaTime;

        if (!knock.GetIsInoperable())
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0, moveDirection.y * Time.deltaTime, 0));
        }

        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }
    }

    private IEnumerator Attack()
    {
        scissors1.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(0.3f);

        scissors1.GetComponent<Collider>().enabled = false;

        scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
    }

    private void AttackMotion()
    {
        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Space))
        {
            LeaveTime = 0.0f;
            anime.SetTrigger("Attack");

            StartCoroutine(Attack());
        }

        if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
        {
            scissors1.GetComponent<Collider>().enabled = true;
        }
        else
        {
            scissors1.GetComponent<Collider>().enabled = false;
            scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Item")
        {
            iteminfo = other.gameObject.GetComponent<ItemInfo>();
            myitem.AddItem(iteminfo.itemData.GetItemType());

            StaticItem.IsUpdate = true;

            //Debug.Log(iteminfo.itemData.GetItemType());

            Destroy(other.gameObject);
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    private bool CheckGrounded()
    {
        var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

        return Physics.Raycast(ray, rayLength, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = CheckGrounded() ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }
}