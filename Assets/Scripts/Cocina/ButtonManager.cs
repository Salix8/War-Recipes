using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static bool IsMouseOverButton = false;

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer iconRenderer;

    [Range(0f, 1f)]
    public float inactiveOpacity = 0.7f;
    [Range(0f, 1f)]
    public float activeOpacity = 1f;

    void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Encuentra el icono hijo
        Transform icon = transform.Find("Icon");
        if (icon != null)
        {
            iconRenderer = icon.GetComponent<SpriteRenderer>();
        }
    }

    void OnEnable()
    {
        // Al activarse el objeto, asegúrate de aplicar la opacidad inactiva
        ApplyInactiveOpacity();
    }

    void Update()
    {
        if (mainCamera == null) return;

        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    void OnMouseEnter()
    {
        IsMouseOverButton = true;
        SetOpacity(activeOpacity);
    }

    void OnMouseExit()
    {
        IsMouseOverButton = false;
        SetOpacity(inactiveOpacity);
    }

    private void SetOpacity(float opacity)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;
        }

        if (iconRenderer != null)
        {
            Color iconColor = iconRenderer.color;
            iconColor.a = opacity;
            iconRenderer.color = iconColor;
        }
    }

    public void ApplyInactiveOpacity()
    {
        SetOpacity(inactiveOpacity);
    }
}
