using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static bool IsMouseOverButton = false;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetButtonOpacity(0.1f); // Opacidad baja al inicio
    }

    void Update()
    {
        // Hacer que el botón siempre mire a la cámara
        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    void OnMouseEnter()
    {
        IsMouseOverButton = true;
        SetButtonOpacity(1f);
    }

    void OnMouseExit()
    {
        IsMouseOverButton = false;
        SetButtonOpacity(0.1f);
    }

    private void SetButtonOpacity(float opacity)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;
        }
    }
}
