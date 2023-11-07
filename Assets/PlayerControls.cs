using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Public to allow changing speed in Unity Inspector
    public float jumpSpeed = 15f;
    public Rigidbody2D rb;
    public float buttonTime = 0.3f;
    public float jumpAmount = 20;
    float jumpTime;
    bool jumping;
    // Update is called once per frame
    void Update()
    {
        // Input detection for 'WASD' keys
        float moveX = Input.GetAxisRaw("Horizontal"); // 'A' 'D' keys and left/right arrow keys
        float moveY = Input.GetAxisRaw("Vertical"); // 'W' 'S' keys and up/down arrow keys
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jumping = true;
            jumpTime = 0;
        }
        if(jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
            jumpTime += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
        {
            jumping = false;
        }

        // Movement vector
        Vector2 move = new Vector2(moveX, moveY).normalized; // Normalized to maintain constant speed in all directions

        // Apply the movement to the player's position
        transform.position += new Vector3(move.x, move.y, 0) * moveSpeed * Time.deltaTime;
    }
}
