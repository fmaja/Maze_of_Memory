using UnityEngine;
using UnityEngine.AI;

public class EnemyController2 : MonoBehaviour
{
    public Transform[] waypoints;
    public float sightDistance = 10f;
    public float walkSpeed = 2f;
    public float chaseSpeed = 4f;
    public float attackDistance = 1.8f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public AudioClip walkSound;
    public AudioClip chaseSound;
    public AudioClip attackSound;

    public AudioClip eliksirSound; 
    private bool isPacified = false;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform player;

    private int currentWaypoint = 0;
    private bool canSeePlayer = false;
    private float lastAttackTime;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    private PlayerHealth playerHealth;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            player = playerHealth.transform;
        }
        else
        {
            Debug.LogError("PlayerHealth component not found in the scene. Please ensure the player has a PlayerHealth script attached.");
        }

        agent.speed = walkSpeed;
        GoToNextWaypoint();
        PlaySound(walkSound);
    }

    private void Update()
    {
        try
        {
            if (isPacified)
            {
                agent.isStopped = true;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);
                return; 
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (canSeePlayer)
            {
                if (distanceToPlayer <= attackDistance)
                {
                    currentState = State.Attack;
                    agent.isStopped = true;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsChasing", false);
                    animator.SetTrigger("Attack");

                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time;
                    }
                }
                else if (distanceToPlayer <= sightDistance)
                {
                    currentState = State.Chase;
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;
                    agent.SetDestination(player.position);
                    animator.SetBool("IsChasing", true);
                    animator.SetBool("IsWalking", false);
                    PlaySound(chaseSound);
                }
                else
                {
                    canSeePlayer = false;
                    ReturnToPatrol();
                }
            }
            else
            {
                if (currentState == State.Chase || currentState == State.Attack)
                {
                    ReturnToPatrol();
                }
                else if (currentState == State.Patrol)
                {
                    animator.SetBool("IsChasing", false);
                    animator.SetBool("IsWalking", true);

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                        agent.SetDestination(waypoints[currentWaypoint].position);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EnemyController exception: " + e.Message);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPacified) return;

        if (other.CompareTag("Player"))
        {
            canSeePlayer = true;
        }

        if (other.CompareTag("Eliksir"))
        {
            isPacified = true;

            if (eliksirSound != null)
            {
                audioSource.PlayOneShot(eliksirSound);
            }

            Destroy(other.gameObject); 

            agent.isStopped = true;
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsChasing", false);
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsChasing", false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            PlaySound(attackSound);
        }
    }

    private void ReturnToPatrol()
    {
        currentState = State.Patrol;
        agent.speed = walkSpeed;
        agent.isStopped = false;
        GoToNextWaypoint();
        PlaySound(walkSound);
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsChasing", false);
    }
}
