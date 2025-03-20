using UnityEngine;
using UnityEngine.AI;

public class SmoothFollowBox : MonoBehaviour
{
    public Transform target;           // El personaje al que seguirá la caja
    public float smoothTime = 0.2f;    // Tiempo de suavizado
    public float followDistance = 1.5f;// Distancia deseada desde el personaje
    public float rotationSpeed = 5f;   // Velocidad de rodeo alrededor del personaje
    public float bufferDistance = 0.5f;// Distancia mínima antes de rodear
    public float hoverHeight = 1.0f;   // Altura de levitación
    public float hoverSpeed = 2.0f;    // Velocidad de la oscilación
    public Vector3 initialOffset = new Vector3(0, 0, -2.12f);  // Offset inicial desde el personaje

    private Vector3 velocity = Vector3.zero;
    private NavMeshAgent agent;
    private Vector3 floatOffset;       // Desplazamiento para simular la levitación

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("El objeto no tiene un NavMeshAgent. Añádelo desde el Inspector.");
        }

        // Inicializa el offset flotante
        floatOffset = new Vector3(0, hoverHeight, 0);

        // Ajustar la posición inicial con el offset especificado
        transform.position = target.position + initialOffset;
    }

    void Update()
    {
        if (agent == null || target == null) return;

        // Calcular la posición deseada detrás del personaje con el offset inicial
        Vector3 targetPosition = target.position + initialOffset;

        // Calcular la distancia actual a la posición objetivo
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Si está demasiado cerca, rodear suavemente
        if (distanceToTarget < followDistance - bufferDistance)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            direction = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * direction;
            Vector3 newPosition = target.position + direction * followDistance + initialOffset;
            agent.SetDestination(newPosition);
        }
        else
        {
            agent.SetDestination(targetPosition);
        }

        // Rotar la caja para mirar hacia el personaje
        Vector3 lookDirection = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Aplicar movimiento de flotación
        floatOffset.y = hoverHeight + Mathf.Sin(Time.time * hoverSpeed) * 0.2f;
        transform.position = agent.transform.position + floatOffset;
    }
}
