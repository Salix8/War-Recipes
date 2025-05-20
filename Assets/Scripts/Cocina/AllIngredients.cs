using UnityEngine;

public class AllIngredients : MonoBehaviour
{
    [Header("Ingredientes que se extraen")]
    [SerializeField] private Ingredient[] ingredientesExtraibles;

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
            foreach(Ingredient item in ingredientesExtraibles)
                ItemManager.AnyadirIngrediente(item);
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
