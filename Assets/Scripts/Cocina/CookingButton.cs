using UnityEngine;

public class CookingButton : MonoBehaviour, IButtonAction
{
    public CookingInteractable cookingInteractable;
    public GameObject[] secondaryButtons;
    private bool isExpanded = false;

    private void OnMouseDown()
    {
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
        else
        {
            if (cookingInteractable != null)
            {
                cookingInteractable.OnButtonClick(); // Esto abrirá recetas según la estación
            }
        }
    }

    private void ShowSecondaryButtons()
    {
        foreach (var button in secondaryButtons)
        {
            if (button != null)
            {
                button.SetActive(true);

                ButtonManager manager = button.GetComponent<ButtonManager>();
                if (manager != null)
                {
                    manager.ApplyInactiveOpacity(); // Estética visual
                }
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
