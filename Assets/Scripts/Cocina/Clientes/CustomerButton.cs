using UnityEngine;

public class CustomerButton : MonoBehaviour, IButtonAction
{
    public CustomerManager customerManager;

    private void Start()
    {
        if (customerManager == null)
        {
            customerManager = FindFirstObjectByType<CustomerManager>(); // Encuentra el CustomerManager en la escena
        }
        
        if (customerManager == null)
        {
            Debug.LogError("CustomerManager no asignado en CustomerButton");
        }
    }

    private void OnMouseDown()
    {
        OnButtonClick();
    }

public void OnButtonClick()
{
    if (customerManager != null)
    {
        if (customerManager.GetWaitingQueueCount() > 0) // Asegúrate de que haya clientes en la cola
        {
            customerManager.AssignCustomersToTables();
        }
        else
        {
            Debug.LogWarning("No hay clientes en la cola para asignar a las mesas.");
        }
    }
    else
    {
        Debug.LogError("CustomerManager es null.");
    }
}

}

