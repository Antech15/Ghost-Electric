using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControl : EnemyFSM
{
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Speed of the enemy
    private float curSpeed;

    //Whether the NPC is destroyed or not
    private bool bDead;
    private int health;

    public GameObject player;
    protected float distance;

    protected override void Initialize()
    {
        curState = FSMState.Patrol;
        curSpeed = 150.0f;
        bDead = false;
        //elapsedTime = 0.0f;
        health = 100;
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Update the time
        // elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    protected void UpdatePatrolState()
    {
        distance = playerDistance();
        if (distance < 25.0f && distance > 10.0f)
            curState = FSMState.Chase;
        else if (distance < 10.0f)
            curState = FSMState.Attack;
        else
            curState = FSMState.Patrol;

    }

    protected void UpdateChaseState()
    { 
    
    }
    protected void UpdateAttackState()
    {
      
    }
    protected void UpdateDeadState()
    {
        curState = FSMState.Dead;

    }

    protected float playerDistance()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist;
    }
}
