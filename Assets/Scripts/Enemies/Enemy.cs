using System.Collections;
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
    protected Animator animator;

    [Header("Patrullaje")]
    public float patrolRadius = 10f;
    public float waitTimeAtPoint = 2f;
    private float waitTimer = 0f;
    private Vector3 patrolTarget;

    public virtual void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if(animator == null)
            animator = GetComponentInChildren<Animator>();

        PickNewPatrolPoint();
    }

    public virtual void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (animator != null)
        {
            float speed = agent.velocity.magnitude / agent.speed; // normalizamos entre 0 y 1
            animator.SetFloat("Speed", speed);
        }

        if (!isAggro && distToPlayer <= detectionRange)
        {
            isAggro = true;
            OnAggro();
        }

        if (!isAggro)
        {
            PatrolBehavior();
        }
        else
        {
            Act();
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

    public abstract void Act();

    protected abstract void OnAggro();

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die() // soltar loot
    {

        if (loot != null && loot.ingredientInstance != null)
            Instantiate(loot.ingredientInstance.gameObject, transform.position, Quaternion.identity);

        if (animator != null)
            animator.SetTrigger("Die");

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        float animLength = 1f; // Valor por defecto
        if (animator != null)
            animLength = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animLength);
        Destroy(transform.parent.gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
