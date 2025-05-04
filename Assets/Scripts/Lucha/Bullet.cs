using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;
    public float lifetime = 3f;
    public GameObject impactEffect;
    public float antiGravityForce = 2f;
    public float proximityDamageRadius = 1.5f; // Radio para daño cercano al jugador

    private Rigidbody rb;
    private bool hasHit = false;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Invoke(nameof(Expire), lifetime); // Usamos Invoke para saber cuándo se destruye sin impacto
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * antiGravityForce, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (impactEffect)
{
    GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
    Destroy(effect, 2f); // que viva el efecto antes de destruirse
}


            hasHit = true;
            Destroy(gameObject);
        }
    }

    private void Expire()
    {
        if (!hasHit && player != null)
        {
            Collider[] enemiesNearby = Physics.OverlapSphere(player.position, proximityDamageRadius, LayerMask.GetMask("Enemy"));

            foreach (Collider col in enemiesNearby)
            {
                EnemyAI enemy = col.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    break; // Solo dañamos al más cercano
                }
            }
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (player != null)
            Gizmos.DrawWireSphere(player.position, proximityDamageRadius);
    }
#endif
}
