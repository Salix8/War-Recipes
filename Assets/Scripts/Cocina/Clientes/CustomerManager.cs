using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab; // Prefab del cliente
    public Transform entryPoint; // Punto de entrada de los clientes
    public float spawnInterval = 5f; // Intervalo de tiempo entre la generación de clientes
    public int maxCustomers = 10; // Número máximo de clientes en el restaurante

    private List<GameObject> customers = new List<GameObject>();
    private TableManager tableManager;

    void Start()
    {
        tableManager = FindObjectOfType<TableManager>();
        if (tableManager == null)
        {
            Debug.LogError("TableManager not found!");
            return;
        }
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (customers.Count < maxCustomers)
            {
                GameObject newCustomer = Instantiate(customerPrefab, entryPoint.position, Quaternion.identity);
                if (NavMesh.SamplePosition(entryPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    newCustomer.transform.position = hit.position;
                    customers.Add(newCustomer);
                    newCustomer.GetComponent<Customer>().Initialize(tableManager, this);
                }
                else
                {
                    Debug.LogError("Failed to place customer on NavMesh.");
                    Destroy(newCustomer);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        customers.Remove(customer);
        Destroy(customer);
    }
}

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private TableManager tableManager;
    private CustomerManager customerManager;

    public void Initialize(TableManager tableManager, CustomerManager customerManager)
    {
        this.tableManager = tableManager;
        this.customerManager = customerManager;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on customer.");
            return;
        }
        MoveToTable();
    }

    private void MoveToTable()
    {
        Transform table = tableManager.GetAvailableTable();
        if (table != null)
        {
            agent.SetDestination(table.position);
        }
        else
        {
            // No hay mesas disponibles, esperar o hacer algo más
        }
    }

    public void LeaveRestaurant()
    {
        customerManager.RemoveCustomer(gameObject);
    }
}