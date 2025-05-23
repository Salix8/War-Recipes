using UnityEngine;

public class CookingInteractable : MonoBehaviour
{
    public GameObject button;
    public Transform destinationPoint; // ← Asigna aquí un GameObject vacío en el Inspector
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (destinationPoint == null)
        {
            Debug.LogError("No has asignado el destino (destinationPoint) para este botón.");
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
        if (button != null && mainCamera != null)
        {
            button.transform.LookAt(mainCamera.transform);
            button.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }

    public void OnButtonClick()
    {
        GameObject character = GameObject.FindWithTag("Player");
        if (character == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'.");
            return;
        }

        if (destinationPoint == null)
        {
            Debug.LogWarning("No se asignó el destino.");
            return;
        }

        CharacterManagerCocina.Instance.MoveToStation(character, destinationPoint);
    }
}
