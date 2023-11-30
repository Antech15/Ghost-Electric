using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantenemycrab : MonoBehaviour
{
        public AudioSource winSound;

    public float walkingSpeed = 2f;
    public float attackRange1 = 2f;
    public float attackRange2 = 1.5f;
    public float jumpInterval = 10f;

    public Transform targetPlayer;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BossBehavior());
        StartCoroutine(Jump());
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        winSound = audioSources[0];
    }

    void Update()
    {
        //when boss is dead, play death animation and destroy object after 2 seconds
        //get the current health from the enemies EnemyHealth script
        if (GetComponent<EnemyHealth>().health <= 0)
        {
            //play win music attached to player
            targetPlayer.GetComponent<PlayerControls>().winSoundd();
        
            animator.SetTrigger("Death");
            Destroy(gameObject, 7f);

            //stop game music
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
        }
    }

    private IEnumerator BossBehavior()
    {
        while (true)
        {
            if (targetPlayer != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

                if (distanceToPlayer > attackRange1 && distanceToPlayer > attackRange2)
                {
                    MoveTowardsPlayer();
                    yield return null;
                }
                else
                {
                    StopMoving();

                    if (distanceToPlayer <= attackRange1 && distanceToPlayer <= attackRange2)
                    {
                        // Both attacks are in range, prioritize the closer one
                        float distanceToAttack1 = Mathf.Abs(distanceToPlayer - attackRange1);
                        float distanceToAttack2 = Mathf.Abs(distanceToPlayer - attackRange2);

                        if (distanceToAttack1 < distanceToAttack2)
                        {
                            Debug.Log("Attacking with Attack1");
                            PerformAttack1();
                        }
                        else
                        {
                            Debug.Log("Attacking with Attack2");
                            PerformAttack2();
                        }
                    }
                    else if (distanceToPlayer <= attackRange1)
                    {
                        Debug.Log("Attacking with Attack1");
                        PerformAttack1();
                    }
                    else if (distanceToPlayer <= attackRange2)
                    {
                        Debug.Log("Attacking with Attack2");
                        PerformAttack2();
                    }

                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                Debug.LogError("Player target not assigned! Drag and drop the player GameObject onto the 'Target Player' field in the inspector.");
                yield return null;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        transform.Translate(direction * walkingSpeed * Time.deltaTime);

        if (direction.x > 0)
            transform.localScale = new Vector3(4.35f, 4.35f, 4.35f);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-4.35f, 4.35f, 4.35f);

        // Lock the rotation on the z-axis
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);

        animator.SetBool("IsWalking", true);
    }

    private void StopMoving()
    {
        animator.SetBool("IsWalking", false);
    }

    private void PerformAttack1()
    {
        animator.SetTrigger("Attack1");

        //call Player's PlayerControls.cs TakeDamage() function
        targetPlayer.GetComponent<PlayerControls>().TakeDamage(20);

    }

    private void PerformAttack2()
    {
        animator.SetTrigger("Attack2");
        targetPlayer.GetComponent<PlayerControls>().TakeDamage(20);
    }

    
    private IEnumerator Jump()
    {
        while (true)
        {
            if (targetPlayer != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

                if (distanceToPlayer > attackRange1 && distanceToPlayer > attackRange2)
                {
                    Debug.Log("Jumping");
                    animator.SetTrigger("Jump");

                    // Stand still for 1 second
                    yield return new WaitForSeconds(1.5f);

                    // Add actual jumping logic here
                    float jumpHeight = 5f;
                    float jumpDuration = 1f;
                    Vector3 startPos = transform.position;
                    Vector3 targetPos = transform.position + Vector3.up * jumpHeight;

                    float elapsedTime = 0f;
                    while (elapsedTime < jumpDuration)
                    {
                        float t = elapsedTime / jumpDuration;
                        transform.position = Vector3.Lerp(startPos, targetPos, t);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    transform.position = targetPos;

                    // Bring the boss back down
                    elapsedTime = 0f;
                    while (elapsedTime < jumpDuration)
                    {
                        float t = elapsedTime / jumpDuration;
                        transform.position = Vector3.Lerp(targetPos, startPos, t);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    transform.position = startPos;

                }
            }

            yield return new WaitForSeconds(jumpInterval);
        }
    }

}
