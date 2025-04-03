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
    private Vector3 seatOffset = new Vector3(-0.15f, 0.05f, 0f); // 📌 Ajuste para sentarse un poco más a la izquierda
    private Rigidbody rb; // 📌 Para desactivar colisiones al sentarse

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Obtener Rigidbody si existe
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
        if (chair == null) return;

        chairTransform = chair;
        TableManager tableManager = FindObjectOfType<TableManager>();
        assignedTableObject = tableManager?.GetTableForChair(chair);

        // 📌 Posición previa para ubicarse antes de sentarse
        Vector3 preSitPosition = chair.position - (chair.forward * 0.6f);
        preSitPosition.y = chair.position.y;

        agent.SetDestination(preSitPosition);
        StartCoroutine(PrepareForSitting());
    }

    private IEnumerator PrepareForSitting()
    {
        // 📌 Esperar a que llegue a la posición previa a la silla
        while (agent.pathPending || agent.remainingDistance > 0.2f)
        {
            yield return null;
        }

        // 📌 Apagar el NavMeshAgent y desactivar colisiones para evitar problemas al sentarse
        agent.enabled = false;
        if (rb != null) rb.isKinematic = true; // Desactivar colisiones físicas

        // 📌 Girar de espaldas a la silla
        Quaternion targetRotation = Quaternion.LookRotation(-chairTransform.forward);
        yield return SmoothLookAt(targetRotation, 0.3f);

        // 📌 Activar animación de sentarse
        animator.SetTrigger("SitDown");

        // 📌 Esperar la duración de la animación antes de teletransportarlo
        yield return new WaitForSeconds(1.2f);

        // 📌 Posicionarlo correctamente en la silla con desplazamiento a la izquierda
        transform.position = chairTransform.position + seatOffset;
        transform.rotation = chairTransform.rotation;

        // 📌 Marcar como sentado
        isSeated = true;
        animator.SetBool("IsSitting", true);

        // 📌 Girar la parte superior del cuerpo hacia el objeto en la mesa si existe
        if (assignedTableObject != null)
        {
            StartCoroutine(TorsoLookAt(assignedTableObject.position));
        }
    }

    private IEnumerator SmoothLookAt(Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator TorsoLookAt(Vector3 targetPosition)
    {
        // 📌 Solo giramos la parte superior del cuerpo
        float elapsedTime = 0;
        float duration = 0.5f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
