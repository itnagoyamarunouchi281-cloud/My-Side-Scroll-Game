using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("移動")]
    public float jumpPower = 8f;
    public float gravity = 20f;
    [SerializeField] private int maxJumpCount = 2;

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
    private bool isWalking;
    private int jumpCount;

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
        velocity.x = inputX * 5f;

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
        if (jumpCount >= maxJumpCount) return;

        if (isGrounded)
        {
            jumpCount = 0;
        }

        velocity.y = jumpPower;
        isJump = true;
        jumpCount++;
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
            jumpCount = 0;
        }
    }

    void UpdateAnimator()
    {
        anime.SetBool("isWalking", isWalking);
        anime.SetBool("isJumping", isJump);
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