using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SmoothFollowBox : MonoBehaviour
{
    [System.Serializable]
    public class IngredientVisual
    {
        public string ingredientName;
        public GameObject model;
    }

    public static SmoothFollowBox Instance;

    public Transform target;
    public float followDistance = 1.5f;
    public float rotationSpeed = 5f;
    public float bufferDistance = 0.5f;
    public float hoverHeight = 1.0f;
    public float hoverSpeed = 2.0f;
    public Vector3 initialOffset = new Vector3(0, 0, -2.12f);

    public float avoidEnemyRadius = 1.5f;
    public LayerMask enemyLayer;

    public List<IngredientVisual> ingredientVisuals;

    private Dictionary<string, GameObject> ingredientModelDict = new Dictionary<string, GameObject>();
    private GameObject currentVisual;
    private NavMeshAgent agent;
    private Animator anim;

    private Vector3 desiredPosition;
    private Vector3 floatOffset;
    private bool isPaused = false;

    private Transform crateParent;

    void Awake()
    {
        Instance = this;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        floatOffset = new Vector3(0, hoverHeight, 0);

        transform.position = target.position + initialOffset;

        crateParent = transform.Find("Crate");
        if (crateParent == null)
            Debug.LogError("No se encontró el objeto hijo 'Crate'.");

        foreach (var visual in ingredientVisuals)
        {
            if (visual.model != null && !string.IsNullOrEmpty(visual.ingredientName))
            {
                ingredientModelDict[visual.ingredientName] = visual.model;
                visual.model.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (agent == null || target == null) return;

        if (Time.timeScale == 0f)
        {
            // Guardar la posición actual solo una vez
            if (!isPaused)
            {
                isPaused = true;
                agent.isStopped = true;

                // Opcional: desactiva el animator si está causando problemas
                if (anim != null) anim.enabled = false;
            }

            // Mantener posición estática y flotando en el mismo sitio
            Vector3 holdPos = transform.position;
            holdPos.y = target.position.y + hoverHeight + Mathf.Sin(Time.unscaledTime * hoverSpeed) * 0.2f;
            transform.position = holdPos;
            return;
        }
        else if (isPaused)
        {
            isPaused = false;
            agent.isStopped = false;

            if (anim != null) anim.enabled = true;
        }

        // Calcular nueva posición deseada
        desiredPosition = target.position + initialOffset;

        if (Physics.CheckSphere(desiredPosition, avoidEnemyRadius, enemyLayer))
            desiredPosition = FindSafePositionAroundPlayer();

        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (distToTarget < followDistance - bufferDistance)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            direction = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * direction;
            Vector3 newPos = target.position + direction * followDistance + initialOffset;

            if (!Physics.CheckSphere(newPos, avoidEnemyRadius, enemyLayer))
                desiredPosition = newPos;
        }

        agent.SetDestination(desiredPosition);

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
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
                return candidate + initialOffset;
        }
        return target.position + initialOffset;
    }

    public void ShowCollectedItem(string ingredientName)
    {
        if (currentVisual != null) currentVisual.SetActive(false);

        if (ingredientModelDict.TryGetValue(ingredientName, out var model))
        {
            currentVisual = model;
            currentVisual.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No visual model for: " + ingredientName);
        }
    }
}
