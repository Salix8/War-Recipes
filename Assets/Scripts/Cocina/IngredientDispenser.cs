using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IngredientDispenser : MonoBehaviour
{
    [Header("Ingrediente que se extrae")]
    [SerializeField] private Ingredient ingredienteAExtraer;

    [Header("Prefab visual del ingrediente (se instancia)")]
    [SerializeField] private GameObject visualPrefab;

    [Header("UI")]
    [SerializeField] private GameObject promptUI;

    private bool playerCerca = false;

    private void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (playerCerca && Input.GetKeyDown(KeyCode.E))
            TryDispense();
    }

    private void TryDispense()
    {
        if (!ItemManager.TieneIngrediente(ingredienteAExtraer))
            Debug.Log("[IngredientDispenser] El jugador no tiene ese ingrediente.");
        
        ItemManager.EliminarIngrediente(ingredienteAExtraer);
        if (visualPrefab == null)
            Debug.LogWarning("[IngredientDispenser] No se ha asignado visualPrefab.");

        Instantiate(visualPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
