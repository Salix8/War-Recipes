using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarretaSceneHandler : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt;

    [Header("Configuración de escena")]
    [Tooltip("Si es verdadero, se usará BackButton. Si es falso, se usará GameManager con la escena dada.")]
    [SerializeField] private bool goBack = true;

    [Tooltip("Escena a cargar si no se usa BackButton.")]
    [SerializeField] private string sceneToLoad;

    [Tooltip("Referencia opcional al BackButton si se usa para volver.")]
    [SerializeField] private BackButton backButton;

    private bool playerInside = false;

    void Start()
    {
        if (interactPrompt == null)
            interactPrompt = GameObject.Find("GoToCombat");
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (goBack)
            {
                if (backButton != null)
                    backButton.GoBack();
                else
                    Debug.LogWarning("BackButton no asignado en el inspector.");
            }
            else
            {
                if (GameManager.Instance != null && !string.IsNullOrEmpty(sceneToLoad))
                    GameManager.Instance.ChangeScene(sceneToLoad);
                else
                    Debug.LogError("GameManager.Instance es null o no se ha asignado sceneToLoad.");
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
