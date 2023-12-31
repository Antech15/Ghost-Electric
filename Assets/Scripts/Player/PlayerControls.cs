using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    // for animation
    public Animator animator;


    
    public GameObject deathPanel; // Reference to your DeathPanel GameObject // Reference to the DeathPanel GameObject
    public GameObject winPanel; // Reference to your DeathPanel GameObject // Reference to the DeathPanel GameObject
    private Ray ray;
	private RaycastHit2D ray_cast_hit;
    public GameObject fx_prefab;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public AudioSource jumpSound;

    public AudioSource winSound;
    bool PlayerDead = false;
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
    public List<Item> inventory = new List<Item>();
    private Item selectedItem;
    public TMP_Text noteText;

    void Start()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        jumpSound = audioSources[0];
        healSound = audioSources[2];
        winSound = audioSources[3];
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();

        // Freeze rotation along the Z-axis to prevent falling over
        rb.freezeRotation = true;

        deathPanel.SetActive(false);
        winPanel.SetActive(false);
       
    }

    void Update()
    {
        if(PlayerDead)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = 0f;
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isGrounded = IsPlayerGrounded();
        isJumping = !isGrounded;

        // Set IsFalling based on velocity (you may need to adjust this based on your game)
        isFalling = rb.velocity.y < 0 && !isGrounded;

        //animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsFalling", isFalling); // Update the IsFalling parameter
        animator.SetBool("IsGrounded", isGrounded);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            HideNoteOnScreen();
        }

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     InteractWithDialogueTrigger();
        // }
        
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
        int groundLayerMask = LayerMask.GetMask("Ground", "Platform", "platform");
        Vector2 boxSize = new Vector2(0.9f, 0.1f);
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, groundLayerMask);
        return hit != null;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        healthBar.SetHealth(currentHealth);
        if(currentHealth<=0)
        {
            killPlayer();
        }
        
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
        //Instantiate(fx_prefab);
        healthBar.SetHealth(currentHealth);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision Detected");
        if(other.gameObject.CompareTag("Enemy"))
        {
            // The player collided with an object tagged as "Enemy"
            // You can add your logic here, such as playing a sound or taking damage.
            // healSound.Play();
            Debug.Log(other.gameObject.name + "Player touched enemy");
            TakeDamage(20);
        // Destroy(gameObject); // This line would destroy the player, be cautious if this is intended.
        }
    }

    void onHitByBoss()
    {
        TakeDamage(35);
    }

    public void winSoundd()
    {
        winSound.Play();
        winPanel.SetActive(true);
    }

    public void killPlayer()
    {
        deathPanel.SetActive(true);
        PlayerDead = true;
    }

    IEnumerator ResetTrigger(Collider2D collider)
{
    collider.isTrigger = true;
    yield return new WaitForSeconds(0.5f); // wait for 0.5 seconds
    collider.isTrigger = false;
}


public void SelectItem(Item item)
{
    selectedItem = item;

    // If the selected item is a note, display it on the screen
    if (item.isNote)
    {
        DisplayNoteOnScreen(item.description);
    }
    else
    {
        // Implement logic to display information about the selected item
    }
}

private void DisplayNoteOnScreen(string text)
{
    noteText.text = text;
}

private void HideNoteOnScreen()
{
    noteText.text = "";
    // Optionally, perform other actions when hiding the note
}

  private void InteractWithDialogueTrigger()
    {
        // You may want to raycast from the player to check if there's a dialogue trigger in front of them
        // For simplicity, let's assume the player is interacting with the trigger directly

        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            //dialogueTrigger.StartDialogue();
        }
    }
}