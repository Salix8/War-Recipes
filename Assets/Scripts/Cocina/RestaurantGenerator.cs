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
        //SpawnObstacles();
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
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
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
            instancia.transform.SetParent(obstaclesParent);
            colocados++;
        }

        Vector3 TileToWorldPosition(Vector2Int tile)
        {
            return new Vector3(tile.x * tileSize + tileSize / 2f, 0, tile.y * tileSize + tileSize / 2f);
        }
    }
}

    /*void SpawnObstacles()
    {
        for (int i = 0; i < Mathf.Min(requiredObstacles.Length, requiredSpawnPoints.Length); i++)
        {
            Vector3 pos = requiredSpawnPoints[i].position;
            if (!usedPositions.Contains(pos))
            {
                Instantiate(requiredObstacles[i], pos, Quaternion.identity, obstaclesParent);
                usedPositions.Add(pos);
            }
        }

        int maxRandom = Mathf.FloorToInt(width * height * 0.3f);
        int spawned = 0;
        int attempts = 200;

        while (attempts-- > 0 && spawned < maxRandom && randomObstacles.Length > 0)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, height);

            if (x % 2 == 1 && z % 2 == 1)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                if (!usedPositions.Contains(pos))
                {
                    GameObject prefab = randomObstacles[Random.Range(0, randomObstacles.Length)];
                    Instantiate(prefab, pos, Quaternion.identity, obstaclesParent);
                    usedPositions.Add(pos);
                    spawned++;
                }
            }
        }
    }*/


/*using UnityEngine;
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

    [Header("Obstáculos")]
    public GameObject[] requiredObstacles; // Obstáculos que deben aparecer
    public Transform[] requiredSpawnPoints; // Posiciones exactas para ellos

    public GameObject[] randomObstacles; // Obstáculos aleatorios

    private float tileSize = 4f;
    private Transform floorParent;
    private Transform wallsParent;
    private Transform obstaclesParent;
    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

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
        for (int x = -1; x < width + 1; x++)
        {
            for (int z = -1; z < height + 1; z++)
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

        // Bordes horizontales (incluye bottom para dibujar paredes normales)
        for (int x = 0; x < width; x++)
        {
            Vector3 bottomWall = new Vector3(x * tileSize, 0, -1 * tileSize + wallSpacing);
            Vector3 topWall = new Vector3(x * tileSize, 0, height * tileSize - wallSpacing);

            wallPositions.Add(bottomWall);
            wallRotations.Add(Quaternion.identity);

            wallPositions.Add(topWall);
            wallRotations.Add(Quaternion.identity);
        }

        // Bordes verticales
        for (int z = 0; z < height; z++)
        {
            Vector3 leftWall = new Vector3(-1 * tileSize + wallSpacing, 0, z * tileSize);
            Vector3 rightWall = new Vector3(width * tileSize - wallSpacing, 0, z * tileSize);

            wallPositions.Add(leftWall);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));

            wallPositions.Add(rightWall);
            wallRotations.Add(Quaternion.Euler(0, 90, 0));
        }

        // Filtrar solo paredes válidas para puerta y entrega (no fondo inferior)
        List<int> validDoorIndexes = new List<int>();
        for (int i = 0; i < wallPositions.Count; i++)
        {
            if (wallPositions[i].z > -tileSize + 0.1f) // excluir borde inferior por visibilidad
                validDoorIndexes.Add(i);
        }

        int doorIndex = validDoorIndexes[Random.Range(0, validDoorIndexes.Count)];
        Vector3 doorPos = wallPositions[doorIndex];
        Quaternion doorRot = wallRotations[doorIndex];
        wallPositions.RemoveAt(doorIndex);
        wallRotations.RemoveAt(doorIndex);

        int deliveryIndex = validDoorIndexes[Random.Range(0, validDoorIndexes.Count - 1)];
        Vector3 deliveryPos = wallPositions[deliveryIndex];
        Quaternion deliveryRot = wallRotations[deliveryIndex];
        wallPositions.RemoveAt(deliveryIndex);
        wallRotations.RemoveAt(deliveryIndex);

        // Ventanas
        int windowCount = Random.Range(3, 6);
        for (int i = 0; i < windowCount && wallPositions.Count > 0; i++)
        {
            int index = Random.Range(0, wallPositions.Count);
            Instantiate(windowPrefab, wallPositions[index], wallRotations[index], wallsParent);
            wallPositions.RemoveAt(index);
            wallRotations.RemoveAt(index);
        }

        // Resto de paredes
        for (int i = 0; i < wallPositions.Count; i++)
        {
            Instantiate(wallPrefab, wallPositions[i], wallRotations[i], wallsParent);
        }

        Instantiate(doorPrefab, doorPos, doorRot, wallsParent);
        Instantiate(deliveryPrefab, deliveryPos, deliveryRot, wallsParent);
    }

    void SpawnObstacles()
    {
        // Obstáculos necesarios en posiciones fijas
        for (int i = 0; i < Mathf.Min(requiredObstacles.Length, requiredSpawnPoints.Length); i++)
        {
            Vector3 pos = requiredSpawnPoints[i].position;
            if (!usedPositions.Contains(pos))
            {
                Instantiate(requiredObstacles[i], pos, Quaternion.identity, obstaclesParent);
                usedPositions.Add(pos);
            }
        }

        // Obstáculos aleatorios solo en posiciones impares no ocupadas y max 30% del mapa
        int maxRandom = Mathf.FloorToInt(width * height * 0.3f);
        int spawned = 0;
        int intentos = 100;

        while (intentos-- > 0 && spawned < maxRandom && randomObstacles.Length > 0)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, height);
            if (x % 2 == 1 && z % 2 == 1)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                if (!usedPositions.Contains(pos))
                {
                    GameObject prefab = randomObstacles[Random.Range(0, randomObstacles.Length)];
                    Instantiate(prefab, pos, Quaternion.identity, obstaclesParent);
                    usedPositions.Add(pos);
                    spawned++;
                }
            }
        }
    }
}



/*using UnityEngine;
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
*/