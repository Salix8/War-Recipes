using UnityEngine;

public class CookingInteractable : MonoBehaviour
{
    public GameObject button; // Referencia al botón en la escena
    private Camera mainCamera;
    private CookingStation cookingStation;

    void Start()
    {
        mainCamera = Camera.main;
        cookingStation = GetComponent<CookingStation>();

        if (button != null)
        {
            SetButtonOpacity(0.1f); // Muy transparente al inicio
            button.SetActive(true); // Mostrar el botón al inicio

            // Añadir eventos de ratón al botón
            var buttonCollider = button.GetComponent<BoxCollider2D>();
            if (buttonCollider == null)
            {
                buttonCollider = button.AddComponent<BoxCollider2D>();
                buttonCollider.isTrigger = true;
            }
            var buttonScript = button.GetComponent<ButtonManager>();
            if (buttonScript == null)
            {
                buttonScript = button.AddComponent<ButtonManager>();
                buttonScript.cookingInteractable = this;
            }
        }
        else
        {
            Debug.LogError("Button is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (button != null)
        {
            // Orientar el botón hacia la cámara
            button.transform.LookAt(mainCamera.transform);
            button.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }

    private void SetButtonOpacity(float opacity)
    {
        if (button != null)
        {
            Color color = button.GetComponent<SpriteRenderer>().color;
            color.a = opacity;
            button.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void OnButtonClick()
    {
        if (cookingStation != null)
        {
            cookingStation.ShowRecipes();
        }
    }

    public void OnSecondaryButtonClick(GameObject secondaryButton)
    {
        // Mover al personaje al interactuable asociado al botón secundario
        if (secondaryButton != null)
        {
            CharacterManagerCocina.Instance.MoveToStation(secondaryButton.transform);
        }
    }
}
