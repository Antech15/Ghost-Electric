using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb.velocity = transform.right * speed; // This is the same as new Vector2(speed, 0)
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Enemy enemy = hitInfo.GetComponent<Enemy>().TakeDamage(20);
        /*if (enemy != null)
        {
            enemy.TakeDamage(20);
        }*/
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit by arrow");
            player.GetComponent<PlayerControls>().TakeDamage(5);
        }
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
