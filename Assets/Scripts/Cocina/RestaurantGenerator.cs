using UnityEngine;
using System.Collections.Generic;

public class RestaurantGenerator : MonoBehaviour
{
    [Header("Configuración del Suelo")]
    public GameObject floorTilePrefab;
    public int width = 6;
    public int height = 6;

    [Header("Configuración de Obstáculos")]
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    [Header("Configuración de Paredes")]
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public GameObject deliveryPrefab;
    public GameObject windowPrefab;

    private float tileSize = 4f;
    private Transform floorParent;
    private Transform obstaclesParent;
    private Transform wallsParent;

    // Este método es llamado desde GameInitializer
    public void GenerateEnvironment()
    {
        CreateParentObjects();
        GenerateFloor();
        GenerateWalls();
        SpawnObstacles();
    }

    void CreateParentObjects()
    {
        GameObject escenario = GameObject.Find("Escenario") ?? new GameObject("Escenario");

        floorParent = GameObject.Find("Escenario/Floor")?.transform ?? new GameObject("Floor").transform;
        floorParent.SetParent(escenario.transform);

        obstaclesParent = GameObject.Find("Escenario/Obstacles")?.transform ?? new GameObject("Obstacles").transform;
        obstaclesParent.SetParent(escenario.transform);

        wallsParent = GameObject.Find("Escenario/Walls")?.transform ?? new GameObject("Walls").transform;
        wallsParent.SetParent(escenario.transform);
    }

    void GenerateFloor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(floorTilePrefab, position, Quaternion.identity);
                tile.transform.SetParent(floorParent);
            }
        }
    }

    void GenerateWalls()
    {
        List<Vector3> wallPositions = new List<Vector3>();
        List<Quaternion> wallRotations = new List<Quaternion>();

        float wallOffset = 1.2f; // Ajuste para unir los walls (que sea un wall uniforme)
        float wallSpacing = 1.8f; // Espacio de los walls respecto a los floors

        // Genera posiciones y rotaciones para los muros en los bordes del suelo
        for (int x = 0; x < width; x++) // Borde horizontal superior e inferior
        {
            Vector3 bottomWallPos = new Vector3(x * tileSize, 0, -1 * tileSize + wallSpacing);
            Vector3 topWallPos = new Vector3(x * tileSize, 0, height * tileSize - wallSpacing);

            wallPositions.Add(bottomWallPos);
            wallRotations.Add(Quaternion.identity);

            wallPositions.Add(topWallPos);
            wallRotations.Add(Quaternion.identity);
        }

        for (int z = 0; z < height; z++) // Borde vertical izquierdo y derecho
        {
            Vector3 leftWallPos = new Vector3(-1 * tileSize + wallSpacing, 0, z * tileSize);
            Vector3 rightWallPos = new Vector3(width * tileSize - wallSpacing, 0, z * tileSize);

            wallPositions.Add(leftWallPos);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));

            wallPositions.Add(rightWallPos);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));
        }

        // Ubicaciones puerta y zona de entrega
        int randomIndex = Random.Range(0, wallPositions.Count);
        Vector3 doorPosition = wallPositions[randomIndex];
        Quaternion doorRotation = wallRotations[randomIndex];
        wallPositions.RemoveAt(randomIndex);
        wallRotations.RemoveAt(randomIndex);

        randomIndex = Random.Range(0, wallPositions.Count);
        Vector3 deliveryPosition = wallPositions[randomIndex];
        Quaternion deliveryRotation = wallRotations[randomIndex];
        wallPositions.RemoveAt(randomIndex);
        wallRotations.RemoveAt(randomIndex);

        // ubicaciones ventanas
        int windowCount = Random.Range(2, 5);
        List<Vector3> windowPositions = new List<Vector3>();
        List<Quaternion> windowRotations = new List<Quaternion>();

        for (int i = 0; i < windowCount; i++)
        {
            if (wallPositions.Count > 0)
            {
                randomIndex = Random.Range(0, wallPositions.Count);
                windowPositions.Add(wallPositions[randomIndex]);
                windowRotations.Add(wallRotations[randomIndex]);
                wallPositions.RemoveAt(randomIndex);
                wallRotations.RemoveAt(randomIndex);
            }
        }

        // puerta
        GameObject door = Instantiate(doorPrefab, doorPosition, doorRotation);
        door.transform.SetParent(wallsParent);

        // zona de entregas
        GameObject delivery = Instantiate(deliveryPrefab, deliveryPosition, deliveryRotation);
        delivery.transform.SetParent(wallsParent);

        // ventanas
        for (int i = 0; i < windowPositions.Count; i++)
        {
            GameObject window = Instantiate(windowPrefab, windowPositions[i], windowRotations[i]);
            window.transform.SetParent(wallsParent);
        }

        // paredes normales
        for (int i = 0; i < wallPositions.Count; i++)
        {
            GameObject wall = Instantiate(wallPrefab, wallPositions[i], wallRotations[i]);
            wall.transform.SetParent(wallsParent);
        }
    }


    void SpawnObstacles()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (obstaclePrefabs.Length > 0)
            {
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
                obstacle.transform.SetParent(obstaclesParent);
            }
        }
    }
}
