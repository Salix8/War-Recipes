using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Daño del Jugador")]
    [Tooltip("Modificador de daño que se añade a cada ataque")]
    public int damageModifier = 2;

    [Tooltip("Radio de alcance del ataque")]
    public float attackRadius = 1.5f;

    [Tooltip("LayerMask de los enemigos")]
    public LayerMask enemyLayer;

    public void RealizarAtaque1()
    {
        int damage = Random.Range(1, 7) + damageModifier; // 1d6 + mod
        AplicarDañoEnemigos(damage);
    }

    public void RealizarAtaque2()
    {
        int damage = Random.Range(1, 5) + Random.Range(1, 5) + damageModifier; // 2d4 + mod
        AplicarDañoEnemigos(damage);
    }

    private void AplicarDañoEnemigos(int damage)
    {
        Collider[] enemigos = Physics.OverlapSphere(transform.position + transform.forward, attackRadius, enemyLayer);
        foreach (Collider col in enemigos)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, attackRadius);
    }
}
