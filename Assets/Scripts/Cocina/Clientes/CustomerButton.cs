using UnityEngine;

public class CustomerButton : MonoBehaviour, IButtonAction
{
    public CustomerManager customerManager;

    private void OnMouseDown()
    {
        // Llamar al evento de clic para este botón
        OnButtonClick();
    }

    public void OnButtonClick()
    {
        if (customerManager != null)
        {
            customerManager.AssignCustomersToTables();
        }
    }
}
