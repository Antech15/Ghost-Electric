using UnityEngine;

public class MoveText : MonoBehaviour
{
    public float speed; // Speed of the text movement

    void Update()
    {
        // Move the text across the screen by changing its position
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        
        // Optional: Reset text position if it goes off screen
        if (transform.position.x > Screen.width) // Assuming the pivot is centered
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
