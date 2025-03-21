using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;
    public CookingInteractable cookingInteractable;
    public GameObject[] secondaryButtons; // Botones secundarios que aparecerán
    private bool isExpanded = false;
    private Camera mainCamera;
    private static ButtonManager activeButton;

    void Awake()
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

    void Start()
    {
        mainCamera = Camera.main;
        SetButtonOpacity(0.1f); // Muy transparente al inicio
        gameObject.SetActive(true); // Mostrar el botón al inicio

        // Añadir eventos de ratón al botón
        var buttonCollider = gameObject.GetComponent<BoxCollider2D>();
        if (buttonCollider == null)
        {
            buttonCollider = gameObject.AddComponent<BoxCollider2D>();
            buttonCollider.isTrigger = true;
        }
    }

    void Update()
    {
        // Orientar el botón hacia la cámara
        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        if (Input.GetMouseButtonDown(0))
        {
            if (activeButton != null && activeButton != this)
            {
                activeButton.HideSecondaryButtons();
                activeButton = null;
            }
        }
    }

    void OnMouseEnter()
    {
        SetButtonOpacity(1f); // Completamente visible al pasar el ratón
    }

    void OnMouseExit()
    {
        SetButtonOpacity(0.1f); // Muy transparente al quitar el ratón
    }

    void OnMouseDown()
    {
        if (secondaryButtons != null && secondaryButtons.Length > 0)
        {
            if (isExpanded)
            {
                HideSecondaryButtons();
            }
            else
            {
                ShowSecondaryButtons();
            }
            isExpanded = !isExpanded;
        }
        else
        {
            OnButtonClick();
        }
    }

    private void SetButtonOpacity(float opacity)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = opacity;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void ShowSecondaryButtons()
    {
        foreach (var button in secondaryButtons)
        {
            if (button != null)
            {
                button.SetActive(true);
            }
        }
        SetActiveButton(this);
    }

    public void HideSecondaryButtons()
    {
        foreach (var button in secondaryButtons)
        {
            if (button != null)
            {
                button.SetActive(false); // Cambiar a SetActive(false) en lugar de Destroy
            }
        }
    }

    public void SetActiveButton(ButtonManager button)
    {
        if (activeButton != null && activeButton != button)
        {
            activeButton.HideSecondaryButtons();
        }
        activeButton = button;
    }

    public void OnButtonClick()
    {
        if (cookingInteractable != null)
        {
            cookingInteractable.OnButtonClick();
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
