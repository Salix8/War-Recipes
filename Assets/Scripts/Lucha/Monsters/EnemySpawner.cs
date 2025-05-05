using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Configuration")]
    public GameObject enemyPrefab;
    public int initialSpawnCount = 2;

    [Header("Spawn Area")]
    public float spawnRadius = 5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Elimina esto si quieres controlar el spawn manualmente desde afuera
    // private void Start()
    // {
    //     SpawnEnemies(initialSpawnCount);
    // }

public void SpawnEnemies(int count)
{
    // OPCIONAL: Elimina enemigos antiguos por seguridad
foreach (Transform child in transform.root)
{
    if (child.CompareTag("Enemy")) // Asegúrate de que los enemigos tengan este tag
        Destroy(child.gameObject);
}


    for (int i = 0; i < count; i++)
    {
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0f;

        Vector3 spawnPosition = transform.position + randomOffset;

        // Instanciar como hijo del área actual
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform.root);
    }
}


    public void ClearEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
#endif
}
