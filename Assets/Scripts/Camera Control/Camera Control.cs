using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target; // The PLAYER that the camera will FOLLOW
    public float smoothSpeed = 0.125f; // Adjust this to control the camera follow speed
    public Vector3 offset; // Offset from the target position
    public float lowerYOffset = 1.0f; // Adjust this to move the player to the lower half of the view

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Check if the player is jumping
            bool isJumping = target.GetComponent<PlayerControls>().isJumping; // Access 'isJumping' like a variable

            if (isJumping)
            {
                desiredPosition.y = transform.position.y; // Prevent the camera from following the player's vertical position
            }
            else
            {
                desiredPosition.y -= lowerYOffset; // Offset the camera to keep the player in the lower half of the view
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}