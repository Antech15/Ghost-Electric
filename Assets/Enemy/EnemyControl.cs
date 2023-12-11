using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControl : EnemyFSM
{
    
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    //[SerializeField]
    //private BoxCollider Attack1HB;
    //[SerializeField]
    //private BoxCollider Attack2HB;
    bool facingRight;

    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack1,
        Attack2,
        Dead,
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Speed of the enemy
    public float curSpeed;

    //Whether the NPC is destroyed or not
    private int health;

    public GameObject player;
    protected float distance;

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
        curState = FSMState.Patrol;
        curSpeed = 2.0f;
        elapsedTime = 0.0f;
        health = 100;
        facingRight = true;
       
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
        if (health <= 0){
            
            curState = FSMState.Dead;
        }
    }

    protected void UpdatePatrolState()
    {
        Debug.Log("In patrol state");
        distance = playerDistance();
        //Debug.Log("Dist is " + distance);
        if (distance > 5.0f) //should change to distance > 6.0f once chase is finished
        {
            anim.SetBool("isRunning", true);
            //anim.SetBool("isRunning", true);
            curState = FSMState.Patrol;
            if (currentPoint == pointB.transform)
            {
                if (!facingRight)
                    flip();
                rb.velocity = new Vector2(curSpeed, 0);
            }
            else //if(transform.position.x == pointA.transform.position.x)
            {
                if (facingRight)
                    flip();
                rb.velocity = new Vector2(-curSpeed, 0);
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.2f && currentPoint == pointB.transform)
            {
                flip();
                currentPoint = pointA.transform;
               
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.2f && currentPoint == pointA.transform)
            {
                flip();
                currentPoint = pointB.transform;   
            }

        }
        //else if(distance > 4.0f && distance <= 6.0f)
            //curState = FSMState.Chase;
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
        //anim.SetBool("isAttacking1", false);
        //anim.SetBool("isRunning", false);
        if (transform.position.x < player.transform.position.x && !facingRight)
            flip();
        else if (transform.position.x > player.transform.position.x && facingRight)
            flip();

        anim.SetBool("isAttacking2", true);
        distance = playerDistance();
        if (distance > 4.0f)
            curState = FSMState.Patrol;
        //else if (distance > 2.0f && distance <= 4.0f)
        //    curState = FSMState.Attack2;
        else if (distance <= 2.0f)
            curState = FSMState.Attack1;
    }
    protected void UpdateAttack1State() //Boxer: close punch
    {
        //anim.SetBool("isAttacking2", false);
        //anim.SetBool("isRunning", false);
        if (transform.position.x < player.transform.position.x && !facingRight)
            flip();
        else if (transform.position.x > player.transform.position.x && facingRight)
            flip();

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
        if(other.tag == "Player"){
            //lower player health
            Debug.Log("Hit player");
        }
    }
}
