using UnityEngine;

public class ChaserEnemy : Enemy
{
    public float chaseSpeed = 3.5f;

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
        agent.SetDestination(player.position);
    }

    protected override void Die()
    {
        if (loot != null && loot.ingredientInstance != null)
            Instantiate(loot.ingredientInstance.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
