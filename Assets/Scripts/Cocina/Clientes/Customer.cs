using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isSeated = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isSeated)
        {
            animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f);
        }
    }

    public void MoveToSeat(Transform chair)
    {
        if (chair != null)
        {
            agent.SetDestination(chair.position);
            StartCoroutine(SitOnChair(chair));
        }
    }

    private IEnumerator SitOnChair(Transform chair)
    {
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        isSeated = true;
        transform.position = chair.position;
        transform.rotation = chair.rotation;
        animator.SetTrigger("SitDown");
    }
}