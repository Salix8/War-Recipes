using UnityEngine;
using UnityEngine.AI;

public class SmoothFollowBox : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.2f;
    public float followDistance = 1.5f;
    public float rotationSpeed = 5f;
    public float bufferDistance = 0.5f;
    public float hoverHeight = 1.0f;
    public float hoverSpeed = 2.0f;
    public Vector3 initialOffset = new Vector3(0, 0, -2.12f);

    public float avoidEnemyRadius = 1.5f;          // Radio para evitar enemigos
    public LayerMask enemyLayer;                  // Define qué capas son "enemigos"

    private Vector3 velocity = Vector3.zero;
    private NavMeshAgent agent;
    private Vector3 floatOffset;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("El objeto no tiene un NavMeshAgent.");
        }

        floatOffset = new Vector3(0, hoverHeight, 0);
        transform.position = target.position + initialOffset;
    }

    void Update()
    {
        if (agent == null || target == null) return;

        Vector3 desiredPosition = target.position + initialOffset;

        // Verifica si hay enemigos cerca del destino
        if (Physics.CheckSphere(desiredPosition, avoidEnemyRadius, enemyLayer))
        {
            // Buscar nueva posición alrededor del personaje
            desiredPosition = FindSafePositionAroundPlayer();
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < followDistance - bufferDistance)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            direction = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * direction;
            Vector3 newPosition = target.position + direction * followDistance + initialOffset;

            if (!Physics.CheckSphere(newPosition, avoidEnemyRadius, enemyLayer))
            {
                desiredPosition = newPosition;
            }
        }

        // Evitar interponerse entre jugador y enemigos
        Collider[] enemies = Physics.OverlapSphere(target.position, 10f, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            Vector3 dirToEnemy = (enemy.transform.position - target.position).normalized;
            Vector3 dirToBox = (transform.position - target.position).normalized;

            // Comprobamos si la box está más cerca que el enemigo en la misma dirección
            float dot = Vector3.Dot(dirToEnemy, dirToBox);
            float distToBox = Vector3.Distance(target.position, transform.position);
            float distToEnemy = Vector3.Distance(target.position, enemy.transform.position);

            if (dot > 0.9f && distToBox < distToEnemy) // está en la línea y más cerca
            {
                // Reposicionar la box a un lado
                Vector3 right = Vector3.Cross(Vector3.up, dirToEnemy).normalized;
                desiredPosition = target.position + right * followDistance + initialOffset;
                break;
            }
        }


        agent.SetDestination(desiredPosition);

        Vector3 lookDirection = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        floatOffset.y = hoverHeight + Mathf.Sin(Time.time * hoverSpeed) * 0.2f;
        transform.position = agent.transform.position + floatOffset;
    }

    Vector3 FindSafePositionAroundPlayer()
    {
        const int attempts = 10;
        for (int i = 0; i < attempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * followDistance;
            Vector3 candidate = target.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (!Physics.CheckSphere(candidate, avoidEnemyRadius, enemyLayer))
            {
                return candidate + initialOffset;
            }
        }

        // Si no encuentra una posición segura, vuelve al destino original
        return target.position + initialOffset;
    }
}
