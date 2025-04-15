using UnityEngine;

public class CowardEnemy : Enemy
{
    public float fleeSpeed = 3f;

    public override void Act()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Vector3 dir = (transform.position - player.position).normalized;
            transform.position += dir * fleeSpeed * Time.deltaTime;
        }
    }

    protected override void Die()
    {
        Instantiate(loot.ingredientInstance.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
