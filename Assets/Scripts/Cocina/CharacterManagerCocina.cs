using UnityEngine;
using UnityEngine.AI;

public class CharacterManagerCocina : MonoBehaviour
{
    public static CharacterManagerCocina Instance;

    public Transform[] spawnPoints;
    public GameObject[] characterPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

public GameObject SpawnCharacter(int i, int j)
{
    if (i < 0 || i >= characterPrefabs.Length)
    {
        Debug.LogError($"Índice {i} fuera de rango en characterPrefabs. Asegúrate de asignar prefabs y no pasar un índice inválido.");
        return null;
    }

    GameObject character = Instantiate(characterPrefabs[i], new Vector3(i * 2, 0, j * 2), Quaternion.identity);
    return character;
}

    // 🔽 NUEVO MÉTODO PARA MOVER AL PERSONAJE A UNA ESTACIÓN
public void MoveToStation(GameObject character, Transform destination)
{
    NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
    if (agent == null)
    {
        Debug.LogWarning("El personaje no tiene NavMeshAgent.");
        return;
    }

    if (!agent.isOnNavMesh)
    {
        Debug.LogWarning("El personaje no está sobre el NavMesh.");
        return;
    }

    NavMeshHit navHit;
    if (NavMesh.SamplePosition(destination.position, out navHit, 1.0f, NavMesh.AllAreas))
    {
        agent.SetDestination(navHit.position);
    }
    else
    {
        Debug.LogWarning("El punto destino está fuera del NavMesh.");
    }
}




}
