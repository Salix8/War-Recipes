using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float detectionRange;
    public float damage;
    public Ingredient loot;

    [SerializeField] protected Transform player;
    protected bool isAggro = false;
    protected NavMeshAgent agent;

    [Header("Patrullaje")]
    public float patrolRadius = 10f;
    public float waitTimeAtPoint = 2f;
    private float waitTimer = 0f;
    private Vector3 patrolTarget;

    public virtual void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        PickNewPatrolPoint();
    }

    public virtual void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAggro && distToPlayer <= detectionRange)
        {
            isAggro = true;
            OnAggro(); // aquí decidimos si perseguir o huir
        }

        if (!isAggro)
        {
            PatrolBehavior();
        }
        else
        {
            Act(); // comportamiento override
        }
    }

    private void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                PickNewPatrolPoint();
                waitTimer = 0f;
            }
        }
    }

    private void PickNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    public abstract void Act(); // perseguir o huir

    protected abstract void OnAggro(); // comportamiento inicial al entrar en aggro

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die(); // soltar loot

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
