using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Configuration")]
    public GameObject enemyPrefab;           // Prefab del enemigo a spawnear
    public int initialSpawnCount = 2;        // Cuántos enemigos aparecen al inicio

    [Header("Spawn Area")]
    public float spawnRadius = 5f;           // Radio dentro del cual aparecen

    private void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0f; // Mantener en el plano horizontal

        Vector3 spawnPosition = transform.position + randomOffset;
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Se podría enlazar con un sistema de gestión más adelante
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
#endif
}
