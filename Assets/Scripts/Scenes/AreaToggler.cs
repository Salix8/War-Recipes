using UnityEngine;

public class AreaToggler : MonoBehaviour
{
    [Header("Área A (por ejemplo, Cocina)")]
    [SerializeField] private GameObject[] areaAObjects;

    [Header("Área B (por ejemplo, Océano)")]
    [SerializeField] private GameObject oceanAreaPrefab;
    private GameObject currentOceanInstance;

    private bool isAreaAActive = true; // Estado inicial

    public void ToggleAreas()
    {
        isAreaAActive = !isAreaAActive;

        SetActiveObjects(areaAObjects, isAreaAActive);

        if (!isAreaAActive)
        {
            // Cambiar a océano: reiniciar océano
            ResetOceanArea();
        }
        else
        {
            // Volver a cocina: ocultar océano
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
    }

    // Opcional: iniciar en Cocina por defecto
    private void Start()
    {
        SetActiveObjects(areaAObjects, isAreaAActive);
    }
}
