using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : EnemyFSM
{
    private Rigidbody2D rb;
    private Animator anim;
    public float range;//enemy will travel from middle point + range on either side
    private float distTraveled; //tracks how much enemy traveled from middle point
    private Vector2 initialPos; //initial starting position of enemy (middle point)
    bool facingRight; //which way enemy is facing
    public FSMState curState; //Current state that the NPC is reaching
    public float curSpeed; //Speed of the enemy
    public GameObject player;
    protected float distance; //distance from player
    public float attack1LowRange;
    public float attack1HighRange;
    private BoxCollider hitBox;
    public AudioSource attack2Sound;
    public AudioSource attack1Sound;

    //public float attack2LowRange;
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
    }

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        attack1Sound = audioSources[0];
        attack2Sound = audioSources[1];
        // range = 3.1f;
        //curSpeed = 2.0f;
        initialPos = transform.position;
        elapsedTime = 0.0f;
        facingRight = true;
        curState = FSMState.Patrol;
        //if(gameObject.name == "Boxer")
            
        //rb.velocity = new Vector2(curSpeed, 0);
    }
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack1: UpdateAttack1State(); break;
            case FSMState.Attack2: UpdateAttack2State(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;
        attackTime += Time.deltaTime;

    }

    protected void UpdatePatrolState()
    {
        Debug.Log(gameObject.name + " In patrol state");
        distance = playerDistance();
        Debug.Log(gameObject.name + "Player dist is " + distance);
        if (!playerHeight())
        {
            elapsedTime += Time.deltaTime; //used so enemy doesn't flip infinitely at end points
            //anim.SetBool("isAttacking1", false);
            //curState = FSMState.Patrol;
            distTraveled = Vector2.Distance(transform.position, initialPos);
            //Mathf.Abs(distTraveled);
            Debug.Log("distTraveled = " + distTraveled);
            if (distTraveled < range) //within range on left or right side
            {
                //move enemy in same direction it was moving previously
                if (facingRight)
                    rb.velocity = new Vector2(curSpeed, 0);
                else
                    rb.velocity = new Vector2(-curSpeed, 0);
            }

            else if (distTraveled > range && elapsedTime > 0.5f) //passed range and didn't just flip a sec ago
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
            else if (distTraveled > range) //need to move enemy back to center to patrol again
            {
                Debug.Log("third");
                if (facingRight)
                    rb.velocity = new Vector2(curSpeed, 0);
                else
                    rb.velocity = new Vector2(-curSpeed, 0);
            }
        }
        else if (playerHeight() && playerInEnemyRange())
            curState = FSMState.Chase;
    }
    protected void UpdateChaseState()
    {
        Debug.Log(gameObject.name + "In chase state");
        distance = playerDistance();
        if (!playerHeight() || !playerInEnemyRange())
            curState = FSMState.Patrol;
        else if (playerHeight() && playerInEnemyRange() && distance < attack1HighRange && distance > attack1LowRange)
            curState = FSMState.Attack1;
        else if (playerHeight() && playerInEnemyRange() && distance <= attack1LowRange)
            curState = FSMState.Attack2;
        else if (playerHeight() && playerInEnemyRange())
        {
            facePlayer();
            if (facingRight)
                rb.velocity = new Vector2(curSpeed, 0);
            else
                rb.velocity = new Vector2(-curSpeed, 0);
        }

    }
    protected void UpdateAttack1State()
    {
        Debug.Log(gameObject.name + "In attack1 state");
        facePlayer();
        attack1Sound.Play();
        anim.SetTrigger("isAttacking1");
        attack1Sound.Play();
        curState = FSMState.Chase;


    }
    protected void UpdateAttack2State()
    {
        Debug.Log(gameObject.name + "In attack2 state");
        facePlayer();
        attack2Sound.Play();
        anim.SetTrigger("isAttacking2");
        //yield return new WaitForSeconds(14);
       // anim.SetBool("isAttacking2", false);
        curState = FSMState.Chase;
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
        if (transform.position.y < (player.transform.position.y + 0.65f) && (player.transform.position.y - 0.65f) < transform.position.y)
        {
            Debug.Log(gameObject.name + "Player height true");
            return true;
        }
        else
        {
            Debug.Log(gameObject.name + "Player height false");
            return false;
        }
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

    private bool playerInEnemyRange()
    {
        float leftRange = initialPos.x - range;
        float rightRange = initialPos.x + range;
        //Debug.Log("Left range:" + leftRange + " rightRange:" + rightRange);
        if (player.transform.position.x <= rightRange && leftRange <= rightRange)
        {
            //Debug.Log("playerinEnemyRange() is true")
            return true;
        }
        else
        {
            //Debug.Log("playerinEnemyRange() is false")
            return false;
        }
    }
}
