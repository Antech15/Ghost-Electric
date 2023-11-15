using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // for animation
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpSpeed = 15f;
    public float runSpeed = 10f;
    public Rigidbody2D rb;
    public float buttonTime = 0.3f;
    public float jumpAmount = 20;
    private float jumpTime;
    public bool isGrounded;
    public bool isRunning;
    public bool isJumping = false;
    public bool isFalling = false; // New variable for falling
    private bool isFacingRight = true; // New variable to track facing direction


    void Start()
    {
        animator = GetComponent<Animator>();

        // Freeze rotation along the Z-axis to prevent falling over
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = 0f;
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isGrounded = IsPlayerGrounded();
        isJumping = !isGrounded;

        // Set IsFalling based on velocity (you may need to adjust this based on your game)
        isFalling = rb.velocity.y < 0 && !isGrounded;

        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsFalling", isFalling); // Update the IsFalling parameter

        if (moveX != 0)
        {
            FlipSprite(moveX);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
        }

        Vector2 move = new Vector2(moveX, moveY).normalized;

        if (isRunning)
        {
            transform.position += new Vector3(move.x, move.y, 0) * runSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(move.x, move.y, 0) * moveSpeed * Time.deltaTime;
        }
    }

    private void FlipSprite(float moveX)
    {
        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            // Flip the sprite by rotating the entire object
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }


    private bool IsPlayerGrounded()
    {
        int groundLayerMask = LayerMask.GetMask("Ground");
        Vector2 boxSize = new Vector2(0.9f, 0.1f);
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, groundLayerMask);
        return hit != null;
    }
}