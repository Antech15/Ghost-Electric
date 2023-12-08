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
    bool facingRight; //which way enemy is facing
    public FSMState curState; //Current state that the NPC is reaching
    public float curSpeed; //Speed of the enemy
    private int health;
    public GameObject player;
    protected float distance; //distance from player
    private int hits = 0;
    public Transform arrowFirePoint;
    public GameObject arrow;



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
        //anim.SetBool("isRunning", true);

        // range = 3.1f;
        //initialPos = transform.position;
        curSpeed = 2.0f;
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
            case FSMState.Attack2: UpdateAttack2State(); break;
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
        Debug.Log("inIdleState");
        anim.SetBool("isShooting", false);
        if (playerHeight())
            curState = FSMState.Attack1;

    }

    protected void UpdateRunState()
    {
        facePlayer();
        anim.SetBool("isRunning", true);

    }

    protected void UpdateAttack1State() //Boxer: close punch
    {
        Debug.Log("In attack1 state");
        facePlayer();
        Debug.Log(attackTime);
        if (attackTime > 1)
        {
            attackTime = 0;
            Instantiate(arrow, arrowFirePoint.position, arrowFirePoint.rotation);
        }
        anim.SetBool("isShooting", true);
        if (hits > 3)
        {
            anim.SetBool("isHit", true);
            curState = FSMState.Run;
        }
        else if (!playerHeight())
            curState = FSMState.Idle;
    }

    protected void UpdateAttack2State() //Boxer: far strong punch
    {
        /*attackTime += Time.deltaTime;
        Debug.Log("In attack2 state");
        anim.SetBool("isAttacking1", false);
        anim.SetBool("isRunning", false);
        facePlayer();
        rb.velocity = new Vector2(curSpeed * 1.25f, 0);
        anim.SetBool("isAttacking2", true);
        distance = playerDistance();
        if (distance > 4.0f || transform.position.x > initialPos.x)
            curState = FSMState.Patrol;
        //else if (distance > 2.0f && distance <= 4.0f)
        //    curState = FSMState.Attack2;
        else if (attackTime < 1.5f)
        {
            curState = FSMState.Idle;
        }
        else if (distance <= 2.0f)
            curState = FSMState.Attack1;
        */
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
        Debug.Log("enemy: " + transform.position.y + " player: " + player.transform.position.y);
        if (transform.position.y < (player.transform.position.y + 0.25f) && (player.transform.position.y - 0.25f) < transform.position.y)
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
        if (other.tag == "Player")
        {
            //lower player health
            Debug.Log("Hit player");
        }
    }

    public void OnColliderEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            //lower player health
            Debug.Log("Enemy hit: " + hits);
        }
    }

    //public void shootArrow();
    //{
  
  //  }
}

