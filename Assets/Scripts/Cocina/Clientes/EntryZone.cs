using UnityEngine;
using UnityEngine.UI;

public class EntryZone : MonoBehaviour
{
    private CustomerManager customerManager;
    public Button assignButton;

    void Start()
    {
        customerManager = FindFirstObjectByType<CustomerManager>();
        assignButton.onClick.AddListener(() => customerManager.AssignCustomersToTables());
    }
}
