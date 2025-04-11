using UnityEngine;
using System.Collections.Generic;

public class RestaurantGenerator : MonoBehaviour
{
    [Header("Configuración del Suelo")]
    public GameObject floorTilePrefab;
    public int width = 6;
    public int height = 6;

    [Header("Configuración de Paredes")]
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public GameObject deliveryPrefab;
    public GameObject windowPrefab;
    [SerializeField] private int minWindow = 5;
    [SerializeField] private int maxWindow = 10;

    [Header("Obstáculos")]
    [SerializeField] private GameObject[] obstaclesUtiles;
    [SerializeField] private GameObject[] randomObstacles;
    [SerializeField] private int cantidadObstaculos = 5;



    private float tileSize = 4f;
    private Transform floorParent;
    private Transform wallsParent;
    private Transform obstaclesParent;
    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();
    private Vector3 doorPosition;
    private Quaternion doorRotation;
    private HashSet<Vector2Int> casillasOcupadas = new HashSet<Vector2Int>();

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

        floorParent = new GameObject("Floor").transform;
        floorParent.SetParent(escenario.transform);

        wallsParent = new GameObject("Walls").transform;
        wallsParent.SetParent(escenario.transform);

        obstaclesParent = new GameObject("Obstacles").transform;
        obstaclesParent.SetParent(escenario.transform);
    }

    void GenerateFloor()
    {
        for (int x = -1; x <= width; x++)
        {
            for (int z = -1; z <= height; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                Instantiate(floorTilePrefab, position, Quaternion.identity, floorParent);
                usedPositions.Add(position);
            }
        }
    }

    void GenerateWalls()
    {
        List<Vector3> wallPositions = new List<Vector3>();
        List<Quaternion> wallRotations = new List<Quaternion>();
        float wallSpacing = 1.8f;

        for (int x = 0; x < width; x++)
        {
            Vector3 bottom = new Vector3(x * tileSize, 0, -1 * tileSize + wallSpacing);
            Vector3 top = new Vector3(x * tileSize, 0, height * tileSize - wallSpacing);
            wallPositions.Add(bottom);          wallRotations.Add(Quaternion.identity);
            wallPositions.Add(top);             wallRotations.Add(Quaternion.identity);
        }

        for (int z = 0; z < height; z++)
        {
            Vector3 left = new Vector3(-1 * tileSize + wallSpacing, 0, z * tileSize);
            Vector3 right = new Vector3(width * tileSize - wallSpacing, 0, z * tileSize);
            wallPositions.Add(left);            wallRotations.Add(Quaternion.Euler(0, 90, 0));
            wallPositions.Add(right);           wallRotations.Add(Quaternion.Euler(0, 90, 0));
        }

        // Filtrar válidas para puerta y entrega (no borde inferior)
        List<int> validDoorIndexes = new List<int>();
        for (int i = 0; i < wallPositions.Count; i++)
        {
            Vector3 pos = wallPositions[i];
            if (!(Mathf.Approximately(pos.z, -1 * tileSize + wallSpacing) && Mathf.Approximately(pos.y, 0)))
                validDoorIndexes.Add(i);
        }

        int doorIndex = validDoorIndexes[Random.Range(0, validDoorIndexes.Count)];
        doorPosition = wallPositions[doorIndex];
        doorRotation = wallRotations[doorIndex];
        wallPositions.RemoveAt(doorIndex);
        wallRotations.RemoveAt(doorIndex);
        Instantiate(doorPrefab, doorPosition, doorRotation, wallsParent);

        int deliveryIndex = validDoorIndexes[Random.Range(0, validDoorIndexes.Count - 1)];
        Vector3 deliveryPos = wallPositions[deliveryIndex];
        Quaternion deliveryRot = wallRotations[deliveryIndex];
        wallPositions.RemoveAt(deliveryIndex);
        wallRotations.RemoveAt(deliveryIndex);
        Instantiate(deliveryPrefab, deliveryPos, deliveryRot, wallsParent);

        int windowCount = Random.Range(minWindow, maxWindow);
        for (int i = 0; i < windowCount && wallPositions.Count > 0; i++)
        {
            int index = Random.Range(0, wallPositions.Count);
            Instantiate(windowPrefab, wallPositions[index], wallRotations[index], wallsParent);
            wallPositions.RemoveAt(index);
            wallRotations.RemoveAt(index);
        }

        for (int i = 0; i < wallPositions.Count; i++)
        {
            Instantiate(wallPrefab, wallPositions[i], wallRotations[i], wallsParent);
        }
    }

    public Vector3 GetDoorPosition()
    {
        return doorPosition;
    }

    public Quaternion GetDoorRotation()
    {
        return doorRotation;
    }


    void SpawnObstacles()
    {
        List<Vector2Int> casillasDisponibles = new List<Vector2Int>();

        // Rellenamos la lista de casillas libres dentro del suelo (sin contar los bordes)
        for (int x = 0; x < width-1; x++)
        {
            for (int z = 0; z < height-1; z++)
            {
                Vector2Int tile = new Vector2Int(x, z);
                casillasDisponibles.Add(tile);
            }
        }

        // Colocamos objetos útiles en posiciones aleatorias
        foreach (GameObject objeto in obstaclesUtiles)
        {
            if (casillasDisponibles.Count == 0) break;

            int index = Random.Range(0, casillasDisponibles.Count);
            Vector2Int tile = casillasDisponibles[index];
            casillasDisponibles.RemoveAt(index);
            casillasOcupadas.Add(tile);

            Vector3 posicion = TileToWorldPosition(tile);
            GameObject instancia = Instantiate(objeto, posicion, Quaternion.identity);
            Debug.Log(posicion);
            instancia.transform.SetParent(obstaclesParent);
        }

        // Colocamos obstáculos aleatorios
        int colocados = 0;
        while (colocados < cantidadObstaculos && casillasDisponibles.Count > 0)
        {
            int index = Random.Range(0, casillasDisponibles.Count);
            Vector2Int tile = casillasDisponibles[index];
            casillasDisponibles.RemoveAt(index);
            casillasOcupadas.Add(tile);

            Vector3 posicion = TileToWorldPosition(tile);
            GameObject prefab = randomObstacles[Random.Range(0, randomObstacles.Length)];

            GameObject instancia = Instantiate(prefab, posicion, Quaternion.identity);
            Debug.Log(posicion);
            instancia.transform.SetParent(obstaclesParent);
            colocados++;
        }

        Vector3 TileToWorldPosition(Vector2Int tile)
        {
            float margin = tileSize * 0.3f; // Puedes ajustar esto para más o menos aleatoriedad
            float offsetX = Random.Range(-margin, margin);
            float offsetZ = Random.Range(-margin, margin);

            float x = tile.x * tileSize + tileSize / 2f + offsetX;
            float z = tile.y * tileSize + tileSize / 2f + offsetZ;

            return new Vector3(x, 0f, z);
        }

    }
}