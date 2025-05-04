using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemy : Enemy
{
    public float chaseSpeed = 3.5f;
    public float attackRange = 2f;
    public float attackInnerRange = 1.5f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    public override void Start()
    {
        base.Start();
    }

    protected override void OnAggro()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;
    }

    public override void Act()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        Log(distance + "<=" + attackRange + "&&" + distance + ">=" + attackInnerRange);
        if (distance <= attackRange && distance >= attackInnerRange)
        {
            agent.isStopped = true;
            OrbitAroundPlayer();

            Log("attackTimer: "+ attackTimer);
            if (attackTimer <= 0f)
            {
                Log("TIEMPO DE ATQUE" + attackTimer);
                AttackPlayer();
                attackTimer = attackCooldown;
            }
        }
        //else if (distance < attackInnerRange)         //    Demasiado cerca  alejarse ligeramente
        //{
        //    Vector3 dirAway = (transform.position - player.position).normalized;
        //    Vector3 fleePos = transform.position + dirAway * (attackInnerRange - distance);
        //    agent.SetDestination(fleePos);
        //    agent.isStopped = false;
        //}
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

    }

    private void OrbitAroundPlayer()
    {
        // Calcular vector tangente
        Vector3 toPlayer = player.position - transform.position;
        Vector3 tangent = Vector3.Cross(Vector3.up, toPlayer).normalized;

        // Mover ligeramente en esa dirección tangente
        Vector3 orbitTarget = transform.position + tangent * 0.5f; // puedes ajustar 0.5f para la "velocidad de órbita"

        agent.SetDestination(orbitTarget);
    }

    private void AttackPlayer()
    {
        Log("¡El enemigo ataca al jugador!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Log(playerHealth.ToString());
            playerHealth.TakeDamage(damage);
        }
    }


    [Header("Debug")]
    [Tooltip("Enable to log scene changes and stack status to the console.")]
    public bool enableDebugLogs = true;
    private void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log("[GameManager] " + message);
        }
    }

}
