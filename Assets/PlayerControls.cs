using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Public to allow changing speed in Unity Inspector

    // Update is called once per frame
    void Update()
    {
        // Input detection for 'WASD' keys
        float moveX = Input.GetAxisRaw("Horizontal"); // 'A' 'D' keys and left/right arrow keys
        float moveY = Input.GetAxisRaw("Vertical"); // 'W' 'S' keys and up/down arrow keys

        // Movement vector
        Vector2 move = new Vector2(moveX, moveY).normalized; // Normalized to maintain constant speed in all directions

        // Apply the movement to the player's position
        transform.position += new Vector3(move.x, move.y, 0) * moveSpeed * Time.deltaTime;
    }
}
