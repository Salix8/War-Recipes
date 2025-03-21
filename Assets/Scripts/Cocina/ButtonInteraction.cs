using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public CookingInteractable cookingInteractable;

    void OnMouseEnter()
    {
        cookingInteractable.OnMouseEnterButton();
    }

    void OnMouseExit()
    {
        cookingInteractable.OnMouseExitButton();
    }

    void OnMouseDown()
    {
        cookingInteractable.OnButtonClick();
    }
}