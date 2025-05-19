using UnityEngine;

public class CookingInteractable : MonoBehaviour
{
    public GameObject button;
    private Camera mainCamera;

    public Transform stationTransform; // Asigna este Transform en el Inspector (debe tener CookingStation)
    private CookingStation cookingStation;

    void Start()
    {
        mainCamera = Camera.main;

        if (stationTransform == null)
        {
            Debug.LogError("stationTransform no está asignado en el Inspector.");
            return;
        }

        cookingStation = stationTransform.GetComponent<CookingStation>();
        if (cookingStation == null)
        {
            Debug.LogError("El Transform asignado no tiene un componente CookingStation.");
            return;
        }

        if (CharacterManagerCocina.Instance == null)
        {
            Debug.LogError("CharacterManagerCocina.Instance es null. ¿Está el prefab/objeto en la escena?");
            return;
        }

        GameObject character = GameObject.FindWithTag("Player"); // Tu personaje debe tener el tag "Player"
        if (character == null)
        {
            Debug.LogError("No se encontró un personaje con el tag 'Player'.");
            return;
        }


        if (button != null)
        {
            button.SetActive(true);

            var collider = button.GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                collider = button.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }

            var btn = button.GetComponent<CookingButton>();
            if (btn == null) btn = button.AddComponent<CookingButton>();
            btn.cookingInteractable = this;
        }
    }

    void Update()
    {
        if (button != null)
        {
            button.transform.LookAt(mainCamera.transform);
            button.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }

public void OnButtonClick()
{
    if (cookingStation != null)
    {
        cookingStation.ShowRecipes();

        // mover al personaje aquí si querés
        GameObject character = GameObject.FindWithTag("Player");
        if (character != null)
        {
            CharacterManagerCocina.Instance.MoveToStation(character, stationTransform);
        }
    }
}


    public void OnSecondaryButtonClick(GameObject secondaryButton)
    {
        if (secondaryButton != null)
        {
            var manager = FindFirstObjectByType<CharacterManagerCocina>();
            if (manager != null)
            {
                manager.MoveToStation(secondaryButton, stationTransform);
            }
        }
    }
}
