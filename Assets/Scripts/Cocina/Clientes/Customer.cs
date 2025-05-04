using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isSeated = false;
    private Transform assignedTableObject;
    private Transform chairTransform;
    private Vector3 seatOffset = new Vector3(-0.15f, 0.05f, 0f);
    private Rigidbody rb;
    private Coroutine torsoLookCoroutine;

    public System.Action OnSeated; // Evento que se dispara cuando el cliente se sienta


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isSeated && animator != null && agent != null)
        {
            animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f);
        }
    }

    public void MoveToSeat(Transform chair)
    {
        if (chair == null) return;

        chairTransform = chair;
        TableManager tableManager = FindObjectOfType<TableManager>();
        assignedTableObject = tableManager?.GetTableForChair(chair);

        Vector3 preSitPosition = chair.position - (chair.forward * 0.6f);
        preSitPosition.y = chair.position.y;

        if (agent != null)
        {
            agent.SetDestination(preSitPosition);
            StartCoroutine(PrepareForSitting());
        }
    }

    private IEnumerator PrepareForSitting()
    {
        while (agent != null && (agent.pathPending || agent.remainingDistance > 0.2f))
        {
            yield return null;
        }

        if (agent != null) agent.enabled = false;
        if (rb != null) rb.isKinematic = true;

        if (chairTransform != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-chairTransform.forward);
            yield return SmoothLookAt(targetRotation, 0.3f);
        }

        if (animator != null)
        {
            animator.SetTrigger("SitDown");
        }

        yield return new WaitForSeconds(1.2f);

        if (chairTransform != null)
        {
            transform.position = chairTransform.position + seatOffset;
            transform.rotation = chairTransform.rotation;
        }

        isSeated = true;

        if (animator != null)
        {
            animator.SetBool("IsSitting", true);
        }

        if (assignedTableObject != null)
        {
            torsoLookCoroutine = StartCoroutine(TorsoLookAt(assignedTableObject.position));
        }

        OnSeated?.Invoke(); // Dispara el evento una vez sentado

    }

    private IEnumerator SmoothLookAt(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (this == null) yield break;

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator TorsoLookAt(Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsedTime = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        while (elapsedTime < duration)
        {
            if (this == null) yield break;

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
