using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : EnemyFSM
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
    //[SerializeField]
    //private BoxCollider Attack1HB;
    //[SerializeField]
    //private BoxCollider Attack2HB;
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack1,
        Attack2,
        Dead,
    }

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", true);
        
        range = 3.1f;
        initialPos = transform.position;
        curSpeed = 2.0f;
        elapsedTime = 0.0f;
        health = 100;
        facingRight = true;
        curState = FSMState.Patrol;
        //rb.velocity = new Vector2(curSpeed, 0);
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack1: UpdateAttack1State(); break;
            case FSMState.Attack2: UpdateAttack2State(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
        {

            curState = FSMState.Dead;
        }
    }

    protected void UpdatePatrolState()
    {
        Debug.Log("In patrol state");
        distance = playerDistance();
        //Debug.Log("Player dist is " + distance);
        if (distance > 5.0f)
        {
            elapsedTime += Time.deltaTime; //used so enemy doesn't flip infinitely at end points
            anim.SetBool("isAttacking1", false);
            anim.SetBool("isAttacking2", false);
            anim.SetBool("isRunning", true);
            //curState = FSMState.Patrol;
            distTraveled = Vector2.Distance(transform.position, initialPos);
            //Mathf.Abs(distTraveled);
            Debug.Log("distTraveled = " + distTraveled);
            if(distTraveled < range) //within range on left or right side
            {
                //move enemy in same direction it was moving previously
                if (facingRight)
                    rb.velocity = new Vector2(curSpeed, 0); 
                else
                    rb.velocity = new Vector2(-curSpeed, 0);
            }

            else if(distTraveled > range && elapsedTime > 0.5f) //passed range and didn't just flip a sec ago
            {
                elapsedTime = 0.0f;
                //flip animation and change moving direction
                if (!facingRight)
                {
                    flip();
                    rb.velocity = new Vector2(curSpeed, 0);
                   
                }
                else
                {
                    flip();
                    rb.velocity = new Vector2(-curSpeed, 0);
                }
            }
        }
        else if (distance > 2.0f && distance <= 4.0f)
            curState = FSMState.Attack2;
    }

    protected void UpdateChaseState()
    {
        distance = playerDistance();
        if (distance > 10.0f)
            curState = FSMState.Patrol;
        else if (distance < 5.0f)
            curState = FSMState.Chase;


    }
    protected void UpdateAttack2State() //Boxer: far strong punch
    {
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
        else if (distance <= 2.0f)
            curState = FSMState.Attack1;
    }
    protected void UpdateAttack1State() //Boxer: close punch
    {

        Debug.Log("In attack1 state");
        anim.SetBool("isAttacking2", false);
        anim.SetBool("isRunning", false);
        facePlayer();
        anim.SetBool("isAttacking1", true);
        distance = playerDistance();
        if (distance > 4.0f)
            curState = FSMState.Patrol;
        else if (distance > 2.0f)
            curState = FSMState.Attack2;
        //else if (distance <= 2.0f)
        //    curState = FSMState.Attack1;
    }


    protected void UpdateDeadState()
    {

        curState = FSMState.Dead;
        // scoreCount.smallElim();

    }

    protected float playerDistance()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        return dist;
    }
    protected void jump()
    {

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
}

