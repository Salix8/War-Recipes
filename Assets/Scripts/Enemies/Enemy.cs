using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float detectionRange;
    public float damage;
    public Ingredient loot;

    [SerializeField] protected Transform player;
    protected bool isAggro;

    public virtual void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Act(); // patrullar, huir o atacar según el tipo

    protected abstract void Die(); // soltar ingrediente

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
