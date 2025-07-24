using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HareAI : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public Transform targetAfterTrigger;     
    public bool followWolf = false;        
    public float minDistance = 0.5f;
    public AudioClip efekt;

    private int currentPoint = 0;
    private bool triggered = false;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayed = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Count > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        if (!triggered)
        {
            Patrol();
        }
        else
        {
            FollowTarget();
        }

        UpdateAnimation();
    }

    void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < minDistance)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void FollowTarget()
    {
        if (targetAfterTrigger == null) return;

        if (followWolf)
        {
            // Dynamiczne œledzenie celu (np. wilka)
            agent.SetDestination(targetAfterTrigger.position);
        }
        else
        {
     
            if (!agent.pathPending && agent.remainingDistance < minDistance)
            {
    
                agent.ResetPath();
            }
            else
            {
                agent.SetDestination(targetAfterTrigger.position);
            }
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (!isPlayed)
            {
                AudioManager.Instance.PlaySFX(efekt);
                isPlayed = true;
            }

            if (!followWolf && targetAfterTrigger != null)
            {
                agent.SetDestination(targetAfterTrigger.position);
                
}
        }
    }
}
