using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float spinSpeed = 150f;
    //public AudioSource healSound;
    public PlayerControls player;
    public GameObject fx_prefab;
    

    void Start()
    {
        //healSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 currentPosition = transform.position;

        // Convert the Vector2 position to a Transform
        Transform spawnTransform = new GameObject().transform;
        spawnTransform.position = currentPosition;

       
        if(collision.CompareTag("Player"))
        {
            Instantiate(fx_prefab, spawnTransform);
            //healSound.Play();
            player.add25();
            gameObject.SetActive(false);
            new WaitForSeconds(3);
            //Destroy(gameObject);
        }
        
    }
    void Update()
    {
        // Rotate the object continuously around the Z-axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}
