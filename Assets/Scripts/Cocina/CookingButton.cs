using UnityEngine;

public class CookingButton : MonoBehaviour, IButtonAction
{
    public CookingInteractable cookingInteractable;
    public GameObject[] secondaryButtons;
    private bool isExpanded = false;

    private void OnMouseDown()
    {
        // Llamar al evento de clic para este botón
        OnButtonClick();
    }

    public void OnButtonClick()
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
        else if (cookingInteractable != null)
        {
            cookingInteractable.OnButtonClick();
        }
    }

    private void ShowSecondaryButtons()
    {
        foreach (var button in secondaryButtons)
        {
            if (button != null)
            {
                button.SetActive(true);
            }
        }
    }

    private void HideSecondaryButtons()
    {
        foreach (var button in secondaryButtons)
        {
            if (button != null)
            {
                button.SetActive(false);
            }
        }
    }
}
