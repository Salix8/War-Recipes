using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] RestaurantGenerator restaurantGenerator;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject spawnPointPlayer;

    private NavMeshSurface navMeshSurface;
    private Transform floorParent;

    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        // Generar el Restaurante (Enviroment)
        restaurantGenerator.GenerateEnvironment();

        // Esperar un frame para asegurarnos de que los objetos han sido creados
        yield return null;

        // Buscar/Crear el NavMeshSurface en "Floor"
        floorParent = GameObject.Find("Escenario/Floor")?.transform;
        if (floorParent == null)
        {
            Debug.LogError("Floor no encontrado o no se ha generado. InitializeGame()");
            yield break;
        }

        GameObject floorObject = floorParent.gameObject;
        // navMeshSurface = floorObject.GetComponent<NavMeshSurface>() ?? floorObject.AddComponent<NavMeshSurface>();

        // Configurar la NavMesh para que use los hijos de Floor
        // navMeshSurface.collectObjects = CollectObjects.Children;

        // Construir la NavMesh
        // navMeshSurface.BuildNavMesh();
        // Debug.Log("NavMesh Generada Correctamente.");

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 spawnPosition = spawnPointPlayer.transform.position;
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Personaje Spawned en: " + spawnPosition);
    }
}
