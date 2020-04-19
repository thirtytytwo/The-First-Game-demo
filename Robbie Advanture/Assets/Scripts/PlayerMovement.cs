using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider;

    [Header("移动参数")]
    public float speed = 8f;
    public float isCrouchDisivor = 3f;

    [Header("跳跃参数")]
    public float jumpForce = 5f;
    public float jumpHoldForce = 1.5f;
    public float crouchJumpBoost = 2.5f;
    public float jumpDuraction = 0.1f;
    public float handingJump = 15f;

    float jumpTime;

    [Header("状态")]
    public bool isCrouch;
    public bool isJump;
    public bool isOnGround;
    public bool isHeadBlock;
    public bool isHanding;

    [Header("环境检测")]
    public LayerMask groundLayer;

    float footOffset = 0.35f;
    float rayDistance = 0.1f;
    float eyeHeight = 1.5f;
    float grabDistance = 0.4f;
    float reachOffset = 0.7f;
    float playerHeight;

    //碰撞体尺寸
    Vector2 standUpSize;
    Vector2 standUpOffset;
    Vector2 crouchSize;
    Vector2 crouchOffset;

    //按键设置
    bool crouchHeld;
    bool jumpPressed;
    bool jumpHeld;

    public float xVelocity;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        //获取站立碰撞体尺寸位置
        standUpSize = boxCollider.size;
        standUpOffset = boxCollider.offset;

        //获取下蹲碰撞体尺寸位置
        crouchSize = new Vector2(boxCollider.size.x, boxCollider.size.y / 2f);
        crouchOffset = new Vector2(boxCollider.offset.x, boxCollider.offset.y / 2f);

        //获取人物高度
        playerHeight = boxCollider.size.y;
    }
    void Update()
    {
        if (GameManager.GameOver())
        {
            return;
        }
        crouchHeld = Input.GetButton("Crouch");
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
    }
    private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            return;
        }
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }
    void GroundMovement()
    {
        
        if (crouchHeld && isOnGround && !isCrouch)
        {
            Crouch();
        }
        else if (!crouchHeld && !isHeadBlock && isCrouch)
        {
            StandUp();
        }
        else if(crouchHeld && !isHeadBlock && !isOnGround)
        {
            StandUp();
        }
        
        xVelocity = Input.GetAxis("Horizontal");
        
        if (isCrouch)
        {
            xVelocity /= isCrouchDisivor;
        }

        rigidbody2D.velocity = new Vector2(xVelocity * speed, rigidbody2D.velocity.y);

        FlipDirection();
    }
    void FlipDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void Crouch()
    {
        isCrouch = true;
        boxCollider.size = crouchSize;
        boxCollider.offset = crouchOffset;
    }
    void StandUp()
    {
        isCrouch = false;
        boxCollider.size = standUpSize;
        boxCollider.offset = standUpOffset;
    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection, color);

        return hit;
    }
    void PhysicsCheck()
    {
        //脚触地检查
        RaycastHit2D leftCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, rayDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, rayDistance, groundLayer);

        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }

        //头部检测
        RaycastHit2D headCheck = Raycast(new Vector2(0f, boxCollider.size.y), Vector2.up, rayDistance, groundLayer);
        if (headCheck)
        {
            isHeadBlock = true;
        }
        else isHeadBlock = false;

        //悬挂检测
        float direction = transform.localScale.x;
        Vector2 grabDirection = new Vector2(direction, 0f);
        
        RaycastHit2D blockCheck = Raycast(new Vector2(footOffset * direction, playerHeight +0.03f), grabDirection, rayDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDirection, rayDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, rayDistance, groundLayer);

        if (!isOnGround && rigidbody2D.velocity.y <= 0f && !blockCheck && ledgeCheck && wallCheck)
        {
            isHanding = true;

            Vector2 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;

            transform.position = pos;
            
            rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
        else isHanding = false;
    }
    void MidAirMovement()
    {
        if (isHanding)
        {
            if (jumpPressed)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.AddForce(new Vector2(0f, handingJump), ForceMode2D.Impulse);
                isHanding = false;
                AudioManager.PlayerJumpAudio();
            }
            if (crouchHeld)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                isHanding = false;
            }
        }
        if (jumpPressed && isOnGround && !isJump)
        {
            isOnGround = false;
            isJump = true;

            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            jumpTime = Time.time + jumpDuraction;

            if (isCrouch && !isHeadBlock)
            {
                StandUp();
                rigidbody2D.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            AudioManager.PlayerJumpAudio();
        }
        else if (isJump)
        {
            if (jumpHeld)
            {
                rigidbody2D.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if(jumpTime< Time.time)
            {
                isJump = false;
            }
        }
    }
}
