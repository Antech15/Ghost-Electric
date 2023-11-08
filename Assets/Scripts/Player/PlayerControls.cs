using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //for animation
    public Animator animator;

    public float moveSpeed = 5f; // Public to allow changing speed in Unity Inspector
    public float jumpSpeed = 15f;
    public float runSpeed = 10f; // Speed while running
    public Rigidbody2D rb;
    public float buttonTime = 0.3f;
    public float jumpAmount = 20;
    private float jumpTime;
    private bool isGrounded; // Tracks whether the player is on the ground
    private bool isRunning; // Tracks whether the player is running
    public bool isJumping = false; //track jumping


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input detection for 'WASD' keys
        float moveX = Input.GetAxisRaw("Horizontal"); // 'A' 'D' keys and left/right arrow keys
        float moveY = 0f; // Set moveY to 0 to prevent jumping with "W" key
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); // Check for Shift key

        // Check if the player is grounded (you need to set up a proper ground detection method)
        isGrounded = IsPlayerGrounded(); // You should implement this method

        //update isjumping based on player input
        isJumping = !isGrounded;

        // Update Animator parameters
        //animator.SetFloat("Speed", Mathf.Abs(moveX));
        //animator.SetBool("IsJumping", !isGrounded);
        //animator.SetBool("IsRunning", isRunning);

        // Jumping is only allowed when grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //animator.setBool("IsJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
        }

        // Movement vector
        Vector2 move = new Vector2(moveX, moveY).normalized;

        // Apply the movement to the player's position
        if (isRunning)
        {
            transform.position += new Vector3(move.x, move.y, 0) * runSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(move.x, move.y, 0) * moveSpeed * Time.deltaTime;
        }
    }

    // Implement your own ground detection logic here
    private bool IsPlayerGrounded()
    {
        // Define a layer mask for your ground objects.
        int groundLayerMask = LayerMask.GetMask("Ground");

        // Define the size of the box collider for ground detection.
        Vector2 boxSize = new Vector2(0.9f, 0.1f); // Adjust these values as needed.

        // Cast a box from the player's position to check for ground collisions.
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, groundLayerMask);

        // If the box overlaps with any ground object, the player is considered grounded.
        return hit != null;
    }
}