using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // GameObject player;

    public float speed = 15f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = -transform.right * speed; // This is the same as new Vector2(speed, 0)
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Enemy enemy = hitInfo.GetComponent<Enemy>().TakeDamage(20);
        /*if (enemy != null)
        {
            enemy.TakeDamage(20);
        }*/
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
