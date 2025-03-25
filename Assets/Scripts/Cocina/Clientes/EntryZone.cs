using UnityEngine;

public class EntryZone : MonoBehaviour
{
    private CustomerManager customerManager;

    void Start()
    {
        customerManager = FindFirstObjectByType<CustomerManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            customerManager.AssignCustomerToTable();
        }
    }
}
