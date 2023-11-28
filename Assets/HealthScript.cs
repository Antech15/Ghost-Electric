using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float spinSpeed = 150f;
    
    public PlayerControls player;
    

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.add25();
            Destroy(gameObject);
        }
        
    }
    void Update()
    {
        // Rotate the object continuously around the Z-axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}