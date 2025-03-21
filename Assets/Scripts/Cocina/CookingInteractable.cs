using UnityEngine;

public class CookingInteractable : MonoBehaviour
{
    public GameObject button; // Referencia al botón en la escena
    private Camera mainCamera;
    private CookingStation cookingStation;

    void Start()
    {
        mainCamera = Camera.main;
        SetButtonOpacity(0.1f); // Muy transparente al inicio
        button.SetActive(true); // Mostrar el botón al inicio
        cookingStation = GetComponent<CookingStation>();

        // Añadir eventos de ratón al botón
        var buttonCollider = button.AddComponent<BoxCollider2D>();
        buttonCollider.isTrigger = true;
        var buttonScript = button.AddComponent<ButtonInteraction>();
        buttonScript.cookingInteractable = this;
    }

    void Update()
    {
        // Orientar el botón hacia la cámara
        button.transform.LookAt(mainCamera.transform);
        button.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    public void OnMouseEnterButton()
    {
        SetButtonOpacity(1f); // Completamente visible al pasar el ratón
    }

    public void OnMouseExitButton()
    {
        SetButtonOpacity(0.1f); // Muy transparente al quitar el ratón
    }

    private void SetButtonOpacity(float opacity)
    {
        Color color = button.GetComponent<SpriteRenderer>().color;
        color.a = opacity;
        button.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnButtonClick()
    {
        if (cookingStation != null)
        {
            cookingStation.ShowRecipes();
        }
    }
}
