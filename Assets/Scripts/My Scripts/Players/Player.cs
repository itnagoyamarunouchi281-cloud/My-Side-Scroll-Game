using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("移動")]
    public float speed = 5f;
    public float jumpPower = 8f;
    public float gravity = 20f;

    [Header("参照")]
    public GameObject charaobj;
    public GameObject camobj;

    [SerializeField] private Animator anime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float rayOffset = 0.5f;

    private CharacterController controller;
    private KnockBack knock;

    private Vector3 velocity;
    private float inputX;
    private bool isGrounded;
    private bool isJump;
    private bool isAttacking;
    private bool isWalking;

    private GameObject scissors1;

    // -------------------------
    // 初期化
    // -------------------------
    void Start()
    {
        controller = GetComponent<CharacterController>();
        knock = GetComponent<KnockBack>();

        scissors1 = GameObject.Find("scissors1");
    }

    // -------------------------
    // 更新
    // -------------------------
    void Update()
    {
        // ===== 入力 =====
        inputX = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Attack();
        }

        // ===== 状態 =====
        CheckGround();

        // ===== 移動 =====
        Move();

        // ===== アニメ更新（ここだけ）=====
        UpdateAnimator();
    }

    // -------------------------
    // 移動処理
    // -------------------------
    void Move()
    {
        // 横移動
        Vector3 move = new Vector3(inputX, 0, 0);

        if (!isGrounded)
        {
            move *= 0.5f; // 空中制御弱め
        }

        controller.Move(move * speed * Time.deltaTime);

        // 向き
        if (inputX > 0)
        {
            charaobj.transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            isWalking = true;
        }
        else if (inputX < 0)
        {
            charaobj.transform.localScale = new Vector3(-1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // 重力
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJump = false;
        }

        velocity.y -= gravity * Time.deltaTime;

        // ノックバック中は横移動禁止
        if (!knock.GetIsInoperable())
        {
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0, velocity.y * Time.deltaTime, 0));
        }

        // Z固定（横スクロール用）
        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    // -------------------------
    // ジャンプ
    // -------------------------
    void Jump()
    {
        if (!isGrounded) return;

        velocity.y = jumpPower;
        isJump = true;
    }

    // -------------------------
    // 攻撃
    // -------------------------
    void Attack()
    {
        isAttacking = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        scissors1.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(0.3f);

        scissors1.GetComponent<Collider>().enabled = false;
    }

    // -------------------------
    // 接地判定
    // -------------------------
    void CheckGround()
    {
        Ray ray = new Ray(
            transform.position + Vector3.up * rayOffset,
            Vector3.down
        );

        isGrounded = Physics.Raycast(ray, rayLength, groundLayer);

        if (isGrounded)
        {
            isJump = false;
        }
    }

    // -------------------------
    // Animator制御（最重要）
    // -------------------------
    void UpdateAnimator()
    {
        float speedParam = Mathf.Abs(inputX);

        if (isAttacking)
        {
            anime.SetTrigger("Attack");
        }
        else
        {
            anime.ResetTrigger("Attack");
        }
        
        anime.SetBool("isWalk", isWalking);
        anime.SetFloat("Speed", speedParam);
        anime.SetBool("isGround", isGrounded);
        anime.SetBool("isJump", isJump);
    }

    // -------------------------
    // デバッグ
    // -------------------------
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }
}