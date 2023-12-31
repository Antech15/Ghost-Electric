using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyGoblin : EnemyFSM
{

    private Rigidbody2D rb;
    private Animator anim;
    public float range;//enemy will travel from middle point + range on either side
    private float distTraveled; //tracks how much enemy traveled from middle point
    private Vector2 initialPos; //initial starting position of enemy (middle point)
    public static bool facingRight; //which way enemy is facing
    public FSMState curState; //Current state that the NPC is reaching
    public float curSpeed; //Speed of the enemy
    private int health;
    public GameObject player;
    protected float distance; //distance from player

    public int hits = 0;
    public Transform arrowFirePoint;
    public AudioSource arrowSound;
    public GameObject arrow;
    public GameObject arrow2;
    //public Arrow arrowScript;


    public enum FSMState
    {
        None,
        Idle,
        Run,
        Attack1,
        Attack2,
        Dead,
    }

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        arrowSound = GetComponent<AudioSource>();
        //arrowScript = arrow.GetComponent<Arrow>();
        //anim.SetBool("isRunning", true);

        // range = 3.1f;
        //initialPos = transform.position;
        elapsedTime = 0.0f;
        health = 100;
        facingRight = true;
        curState = FSMState.Idle;
        //rb.velocity = new Vector2(curSpeed, 0);
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(); break;
            case FSMState.Run: UpdateRunState(); break;
            case FSMState.Attack1: UpdateAttack1State(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;
        attackTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    protected void UpdateIdleState()
    {
        hits = 0;
        Debug.Log("inIdleState");
        anim.SetBool("isShooting", false);
        anim.SetBool("isHit", false);
        //anim.SetBool("isRunning", false);
        if (playerHeight())
            curState = FSMState.Attack1;

    }

    protected void UpdateRunState()
    {
        facePlayer();
        anim.SetBool("isRunning", true);
        if (playerHeight())
        {
            if (facingRight)
                rb.velocity = new Vector2(curSpeed, 0);
            else
                rb.velocity = new Vector2(-curSpeed, 0);
      }
        else
        {
            anim.SetBool("isRunning", false);
            curState = FSMState.Idle;
        }
    }

    protected void UpdateAttack1State() //Boxer: close punch
    {
        //Debug.Log("In attack1 state");
        facePlayer();
        //Debug.Log(attackTime);
        if (attackTime > 1)
        {
            attackTime = 0;
            arrowSound.Play();
            if (facingRight)
                Instantiate(arrow, arrowFirePoint.position, arrowFirePoint.rotation);
            else
                Instantiate(arrow2, arrowFirePoint.position, arrowFirePoint.rotation);
        }
        anim.SetBool("isShooting", true);
        if (hits > 3)
        {
            anim.SetBool("isHit", true);
            //anim.SetBool("isShooting", false);
            curState = FSMState.Run;
        }
        else if (!playerHeight())
            curState = FSMState.Idle;
    }

    protected void UpdateDeadState()
    {
        curState = FSMState.Dead;
    }

    protected float playerDistance()
    {
        float dist = transform.position.x - player.transform.position.x;
        dist = Mathf.Abs(dist);
        return dist;
    }

    protected bool playerHeight()
    {
        //Debug.Log("enemy: " + transform.position.y + " player: " + player.transform.position.y);
        if (transform.position.y < (player.transform.position.y + 1.5f) && (player.transform.position.y - 1.5f) < transform.position.y)
            return true;
        else
            return false;
    }

    private void facePlayer()
    {
        if (transform.position.x < player.transform.position.x && !facingRight)
            flip();
        else if (transform.position.x > player.transform.position.x && facingRight)
            flip();
    }
    private void flip()
    {
        if (facingRight)
            facingRight = false;
        else
            facingRight = true;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Hits is " + hits);
            hits++;
        }
        else if(other.gameObject.CompareTag("Player") && curState == FSMState.Run)
        {
            player.GetComponent<PlayerControls>().TakeDamage(20);
        }
    }

    //public void shootArrow();
    //{
  
  //  }
}

