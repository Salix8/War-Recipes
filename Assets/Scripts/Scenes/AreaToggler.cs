using UnityEngine;

public class AreaToggler : MonoBehaviour
{
    [Header("Área A (por ejemplo, Cocina)")]
    [SerializeField] private GameObject[] areaAObjects;

    [Header("Área B (por ejemplo, Océano)")]
    [SerializeField] private GameObject oceanAreaPrefab;
    private GameObject currentOceanInstance;

    private bool isAreaAActive = true;

    public void ToggleAreas()
    {
        isAreaAActive = !isAreaAActive;

        SetActiveObjects(areaAObjects, isAreaAActive);


        if (!isAreaAActive)
        {
            ResetOceanArea();
            BackgroundMusic.Instance.CambiarEscenario("Oceano");
        }
        else
        {
            BackgroundMusic.Instance.CambiarEscenario("Cocina");
            if (currentOceanInstance != null)
                currentOceanInstance.SetActive(false);
        }
    }

    private void SetActiveObjects(GameObject[] objects, bool value)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(value);
        }
    }

private void ResetOceanArea()
{
    if (currentOceanInstance != null)
        Destroy(currentOceanInstance);

    currentOceanInstance = Instantiate(oceanAreaPrefab);
    currentOceanInstance.SetActive(true);

    // Buscar todos los spawners dentro del nuevo océano y generar enemigos
    EnemySpawner[] spawners = currentOceanInstance.GetComponentsInChildren<EnemySpawner>();
    foreach (var spawner in spawners)
    {
        spawner.SpawnEnemies(spawner.initialSpawnCount);
    }
}


    private void Start()
    {
        SetActiveObjects(areaAObjects, isAreaAActive);
        BackgroundMusic.Instance.CambiarEscenario("Cocina");
    }
}
