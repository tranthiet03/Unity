using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float H_Input;
    private float knockbackStartTime;
    [SerializeField] private float knockbackDuration;
    private int amountJump;

    private bool isFacingRight = true;
    private bool isRunning;
    private bool isTouchingWall;//chạm tường
    private bool isGrounded;
    private bool isWallSliding;// bám tường
    private bool canMove = true;
    private bool canFlip = true;
    private bool canJump;
    private bool isTouchingLedge;


    private bool knockback;

    [SerializeField] private Vector2 knockbackSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJump = 1;//số lần nhảy
    private int facingDirection = 1;
    public float MovementSpeed = 10.0f;//tốc độ di chuyển
    public float JumpForce = 14.0f;//lực nhảy
    public float groundCheckRadius;// Bán kính của GroundCheck hình tròn
    public float wallCheckDistance;//Khoảng cách check tường
    public float wallSlideSpeed;//tốc độ trượt khi bám tường


    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    private EnemyCombatController ECC;
    private int enemyFacingDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountJump = amountOfJump;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckKnockback();
        //TakeDirection();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    private void CheckInput()
    {
        H_Input = Input.GetAxisRaw("Horizontal");

        //Jump
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else if (!isGrounded && isWallSliding)
        {
            if (Input.GetButtonDown("Jump"))
            {
                JumpWall();
            }
        }

    }
    private void ApplyMovement()
    {
        if (!knockback && canMove)
        {
            rb.velocity = new Vector2(MovementSpeed * H_Input, rb.velocity.y);
        }

        if (isWallSliding && !knockback && canMove)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0 || !isGrounded && isTouchingWall && rb.velocity.y <= 0)
        {
            amountJump = amountOfJump;
        }

        if (amountJump <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

    }
    private void Jump()
    {
        if (canJump && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            AudioManager.instance.PlaySFX("Player_Jump");
            amountJump--;
        }
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void JumpWall()
    {
        if (canJump && !knockback && isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * 1.15f);
            AudioManager.instance.PlaySFX("Player_JumpWall");
            amountJump--;
        }
    }

    private void TakeDirection()
    {
        if(ECC.GetPositionEnemy() > transform.position.x)
        {
            enemyFacingDirection = -1;
        }
        else
        {
            enemyFacingDirection = 1;
        }
    }
    public void Knockback()
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * enemyFacingDirection, knockbackSpeed.y);
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

    }
    
   
    private void CheckMovementDirection()
    {
        if (isFacingRight && H_Input < 0)
        {
            Flip();
        }
        else if (!isFacingRight && H_Input > 0)
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void Flip()
    {
        if (canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    public void DisableFlip()//Được sử dụng dùng để khi tấn công thì không cho thay đổi hướng nhìn
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
