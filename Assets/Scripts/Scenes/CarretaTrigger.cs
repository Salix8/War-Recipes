using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class CarretaExitTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt;

    [Header("Referencia al BackButton")]
    [SerializeField] private BackButton backButton; // AQUÍ

    private bool playerInside = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (backButton != null)
            {
                backButton.GoBack();
            }
            else
            {
                Debug.LogWarning("No se ha asignado BackButton en el inspector.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}
