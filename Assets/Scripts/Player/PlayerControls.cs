using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // for animation
    public Animator animator;

    private Ray ray;
	private RaycastHit2D ray_cast_hit;
    public GameObject fx_prefab;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public AudioSource jumpSound;

    public AudioSource healSound;
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
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        jumpSound = audioSources[0];
        healSound = audioSources[2];
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        animator.SetBool("IsGrounded", isGrounded);

        //logic for dropping down through objects wirth the "Platform" Tag and "Ground" Layer when pressing the down arrow or s key twice quickly
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (Time.time < jumpTime + buttonTime)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.tag == "Platform")
                    {
                        StartCoroutine(ResetTrigger(collider));
                    }
                }
            }
            jumpTime = Time.time;
        }

        if (moveX != 0)
        {
            FlipSprite(moveX);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
            TakeDamage(20);
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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
        
    }

    public void add25()
    {
        healSound.Play();
        if(currentHealth+25 > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else{
            
            currentHealth += 25;
            
        }
        Instantiate(fx_prefab);
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator ResetTrigger(Collider2D collider)
{
    collider.isTrigger = true;
    yield return new WaitForSeconds(0.5f); // wait for 0.5 seconds
    collider.isTrigger = false;
}
}
