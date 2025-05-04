using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class CowardEnemy : Enemy
{
    public float fleeSpeed = 3f;
    public float fleeDistance = 10f;
    public float contactDamageCooldown = 1f;
    private bool canDamage = true;

    public override void Start()
    {
        base.Start();
    }

    protected override void OnAggro()
    {
        agent.speed = fleeSpeed;
        agent.isStopped = false;
    }

    public override void Act()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + dir * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            Debug.Log("¡El enemigo hace daño por contacto!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            StartCoroutine(ContactDamageCooldown());
        }
    }

    private System.Collections.IEnumerator ContactDamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(contactDamageCooldown);
        canDamage = true;
    }

}
