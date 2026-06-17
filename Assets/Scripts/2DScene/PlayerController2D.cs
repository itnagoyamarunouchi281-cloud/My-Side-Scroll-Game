using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerController2D : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask groundLayer;
    public float gravity;

    [SerializeField] private float knockbackGravityScale = 1.5f;
    [SerializeField] private float normalGravityScale = 1.0f;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float rayOffset = 0.1f;

    private Rigidbody2D rb2D;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;

    private float leaveTime = 0.0f;
    private const float LeaveThreshold = 5.0f;
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);
    private const float KNOCKBACK_X = 1.8f;
    private const float KNOCKBACK_Y = 0.3f;
    private bool isInvincible = false;
    private bool isKnockback = false;

    public static UnityEvent OnPlayerDamagedEvent = new UnityEvent();

    void Start()
    {
        OnPlayerDamagedEvent.AddListener(() =>
        {
            Vector2 attackDirection = new Vector2(-transform.localScale.x, 0).normalized;
            ApplyKnockback(attackDirection);
        });

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb2D.gravityScale = normalGravityScale;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        animator.SetBool("isWalk", moveInput != 0);

        HandleLeaveMotion();

        isGrounded = CheckGrounded();
        animator.SetBool("isJump", !isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        UpdatePlayerDirection();

        HandleAttack();
    }

    void FixedUpdate()
    {
        rb2D.velocity = new Vector2(moveInput * speed, rb2D.velocity.y);
    }

    private void Jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    private void HandleLeaveMotion()
    {
        if (moveInput == 0 && isGrounded)
        {
            leaveTime += Time.deltaTime;

            if (leaveTime > LeaveThreshold)
            {
                animator.SetBool("isLeave", true);
            }
        }
        else
        {
            leaveTime = 0.0f;
            animator.SetBool("isLeave", false);
        }
    }

    private void UpdatePlayerDirection()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Attack");
        }
    }

    private bool CheckGrounded()
    {
        var origin = new Vector2(transform.position.x, transform.position.y - rayOffset);
        var hit = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - rayOffset), Vector2.down * rayLength);
    }

    private void ApplyKnockback(Vector2 attackDirection)
    {
        if (rb2D != null)
        {
            isKnockback = true;
            rb2D.gravityScale = knockbackGravityScale;

            Vector2 knockback = new Vector2(
                KNOCKBACK_X * attackDirection.x,
                KNOCKBACK_Y
            );
            rb2D.velocity = knockback;

            Invoke(nameof(EndKnockback), 0.5f);
        }
    }

    private void EndKnockback()
    {
        isKnockback = false;
        rb2D.gravityScale = normalGravityScale;
    }
}
