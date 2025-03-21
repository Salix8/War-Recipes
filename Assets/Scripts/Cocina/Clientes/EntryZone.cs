using UnityEngine;

public class EntryZone : MonoBehaviour
{
    private CustomerManager customerManager;

    void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            // Lógica para manejar la entrada del cliente
            // Por ejemplo, puedes iniciar el movimiento del cliente hacia una mesa
            Customer customer = other.GetComponent<Customer>();
            if (customer != null)
            {
                customer.Initialize(customerManager.GetComponent<TableManager>(), customerManager);
            }
        }
    }
}