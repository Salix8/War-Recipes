using UnityEngine;

public class ChaserEnemy : Enemy
{
    public float moveSpeed = 2f;

    public override void Act()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            isAggro = true;
        }

        if (isAggro)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    protected override void Die()
    {
        Instantiate(loot, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
