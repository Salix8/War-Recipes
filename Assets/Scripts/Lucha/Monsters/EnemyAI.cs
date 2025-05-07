using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float damage = 10f;
    private float currentHealth;

    [Header("Detection")]
    public float detectionRadius = 15f;
    public float attackRange = 2f;
    public LayerMask playerLayer;

    [Header("Spawn & Return")]
    public float returnRadius = 5f;
    private Vector3 spawnPosition;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isChasing = false;
    private bool isDead = false;
    private bool isAttacking = false;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAttacking) return;

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;

            if (distanceToPlayer > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetBool("IsWalking", true);
            }
            else
            {
                agent.isStopped = true;
                animator.SetBool("IsWalking", false);
                AttackPlayer();
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                GoBackToSpawn();
            }

            if (!agent.pathPending && agent.remainingDistance <= 0.2f)
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    private void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");

        // Rotar hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        // Aplicar daño al jugador
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Invoke(nameof(EndAttack), 1.0f); // ajusta al tiempo real de la animación
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void GoBackToSpawn()
    {
        Vector3 randomOffset = Random.insideUnitSphere * returnRadius;
        randomOffset.y = 0f;
        Vector3 returnPosition = spawnPosition + randomOffset;

        if (NavMesh.SamplePosition(returnPosition, out NavMeshHit hit, returnRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            agent.isStopped = false;
            animator.SetBool("IsWalking", true);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
        GetComponent<EnemyDrops>().DropItems();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, returnRadius);
    }
#endif
}
