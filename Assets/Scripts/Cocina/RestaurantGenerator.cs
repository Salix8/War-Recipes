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
        for (int x = -1; x < width+1; x++)
        {
            for (int z = -1; z < height+1; z++)
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

        float wallOffsetIncrement = 0.2f; // Ajuste para unir los walls (que sea un wall uniforme)
        float wallSpacing = 1.8f; // Espacio de los walls respecto a los floors

        // Genera posiciones y rotaciones para los muros en los bordes del suelo
        int x;
        float wallOffset;
        for (x = 0, wallOffset = 0f; x < width; x++, wallOffset += wallOffsetIncrement) // Borde horizontal superior e inferior
        {
            Vector3 bottomWallPos = new Vector3(x * tileSize - wallOffset, 0, -1 * tileSize + wallSpacing);
            Vector3 topWallPos = new Vector3(x * tileSize - wallOffset, 0, height * tileSize - wallSpacing);

            wallPositions.Add(bottomWallPos);
            wallRotations.Add(Quaternion.identity);

            wallPositions.Add(topWallPos);
            wallRotations.Add(Quaternion.identity);
        }

        int z;
        for (z = 0, wallOffset = 0f; z < height; z++, wallOffset += wallOffsetIncrement) // Borde vertical izquierdo y derecho
        {
            Vector3 leftWallPos = new Vector3(-1 * tileSize + wallSpacing, 0, z * tileSize - wallOffset);
            Vector3 rightWallPos = new Vector3(width * tileSize - wallSpacing, 0, z * tileSize - wallOffset);

            wallPositions.Add(leftWallPos);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));

            wallPositions.Add(rightWallPos);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));
        }
        // Es importante que reductionFactor se calcule aqui ya que el wallPositions.Count
        Debug.Log($"wallPositions.Count / 4: {wallPositions.Count / 4}\n" +
            $"(wallPositions.Count / 4 * wallOffsetIncrement): {(wallPositions.Count / 4 * wallOffsetIncrement)}\n" +
            $"tileSize - (wallPositions.Count / 4 * wallOffsetIncrement): {tileSize - (wallPositions.Count / 4 * wallOffsetIncrement)}");
        float reductionFactor = (tileSize - (((wallPositions.Count / 4)-1) * wallOffsetIncrement))/4;
        Debug.Log(reductionFactor);
        wallOffset += wallOffsetIncrement;
        reductionFactor = 0.45f;

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
        int windowCount = Random.Range(5, 8);
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

        // Al hacer el WallOffset y juntar todos los muros un poco para que no se vean las marcas se debe crear la ultima esquina
        Debug.Log(reductionFactor);
        Vector3 bottomWallPosLast = new Vector3(x * tileSize - wallOffset - 1f, 0, -1 * tileSize + wallSpacing);
        Vector3 topWallPosLast = new Vector3(x * tileSize - wallOffset - 1f, 0, height * tileSize - wallSpacing);
        GameObject bottomWall = Instantiate(wallPrefab, bottomWallPosLast, Quaternion.identity);
        bottomWall.transform.localScale = new Vector3(reductionFactor, 1f, 1f);
        bottomWall.transform.SetParent(wallsParent);
        GameObject topWall = Instantiate(wallPrefab, topWallPosLast, Quaternion.identity);
        topWall.transform.localScale = new Vector3(reductionFactor, 1f, 1f);
        topWall.transform.SetParent(wallsParent);

        Vector3 leftWallPosLast = new Vector3(-1 * tileSize + wallSpacing, 0, z * tileSize - wallOffset - 1f);
        Vector3 rightWallPosLast = new Vector3(width * tileSize - wallSpacing, 0, z * tileSize - wallOffset - 1f);
        GameObject leftWall = Instantiate(wallPrefab, leftWallPosLast, Quaternion.Euler(0, 90, 0));
        leftWall.transform.localScale = new Vector3(reductionFactor, 1f, 1f);
        leftWall.transform.SetParent(wallsParent);
        GameObject rightWall = Instantiate(wallPrefab, rightWallPosLast, Quaternion.Euler(0, 90, 0));
        rightWall.transform.localScale = new Vector3(reductionFactor, 1f, 1f);
        rightWall.transform.SetParent(wallsParent);
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
