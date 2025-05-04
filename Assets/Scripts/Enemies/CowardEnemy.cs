using UnityEngine;
using UnityEngine.AI;

public class CowardEnemy : Enemy
{
    public float fleeSpeed = 3f;
    public float fleeDistance = 10f;

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

    protected override void Die()
    {
        if (loot != null && loot.ingredientInstance != null)
            Instantiate(loot.ingredientInstance.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
