using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject[] enemyPrefabs;
    public int totalToSpawn = 5;
    public float spawnInterval = 2f;
    public float spawnRadius = 3f;
    public float delayInitialSpawn = 1f;

    private int spawnedCount = 0;
    //private Transform player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(delayInitialSpawn);

        while (spawnedCount < totalToSpawn)
        {
            SpawnEnemy();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 offset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        Vector3 spawnPosition = transform.position + offset;

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
