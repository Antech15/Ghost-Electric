using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arCollectable : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public GameObject childObject1; // Reference to the first child object to enable
    public GameObject childObject2; // Reference to the second child object to enable

    public float hoverSpeed = .25f; // Speed of the hovering motion

    private bool collected = false;

    void Update()
    {
        if (!collected)
        {
            // Make the weapon hover up and down
            float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * 0.001f;
            transform.position = new Vector3(transform.position.x, transform.position.y + hoverOffset, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            // Enable the child objects (ar and arFirePoint)
            if (player != null)
            {
                if(childObject1 != null)
                {
                    childObject1.SetActive(true);
                }

                if (childObject2 != null)
                {
                    childObject2.SetActive(true);
                }

                // Attach your player script here
                Guns guns = player.GetComponent<Guns>();
                if (guns != null)
                {
                    guns.enabled = true;
                }

                // Destroy or deactivate the collectible weapon
                Destroy(gameObject);
            }
        }
    }
}
