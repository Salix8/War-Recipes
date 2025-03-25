using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab; // Prefab del cliente
    public Transform entryPoint; // Punto de entrada de los clientes
    public Transform queueStartPoint; // Punto inicial de la cola
    public float queueSpacing = 1.5f; // Espaciado entre clientes en la cola
    public float spawnInterval = 5f; // Intervalo de generación de clientes
    public int maxCustomers = 10; // Máximo de clientes en la escena
    public int queueSize = 4; // Número máximo de clientes en la cola

    private List<GameObject> customers = new List<GameObject>();
    private Queue<GameObject> waitingQueue = new Queue<GameObject>();
    private TableManager tableManager;

    void Start()
    {
        tableManager = FindFirstObjectByType<TableManager>();
        if (tableManager == null)
        {
            Debug.LogError("TableManager not found!");
            return;
        }
        StartCoroutine(SpawnCustomers());
    }

private IEnumerator SpawnCustomers()
{
    while (customers.Count < maxCustomers)
    {
        Vector3 spawnPos = entryPoint.position;
        NavMeshHit hit;

        // Buscar la posición más cercana en el NavMesh
        if (NavMesh.SamplePosition(spawnPos, out hit, 2.0f, NavMesh.AllAreas))
        {
            GameObject newCustomer = Instantiate(customerPrefab, hit.position, Quaternion.identity);
            customers.Add(newCustomer);
            waitingQueue.Enqueue(newCustomer);
            UpdateQueuePositions();
        }
        else
        {
            Debug.LogError("Failed to place customer on NavMesh.");
        }

        yield return new WaitForSeconds(spawnInterval);
    }
}


    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (GameObject customer in waitingQueue)
        {
            Vector3 queuePosition = queueStartPoint.position + Vector3.back * (index * queueSpacing);
            NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(queuePosition);
            }
            index++;
        }
    }

    public void AssignCustomerToTable()
    {
        if (waitingQueue.Count > 0)
        {
            GameObject customer = waitingQueue.Dequeue();
            Transform chair = tableManager.GetAvailableChair();

            if (chair != null)
            {
                NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.SetDestination(chair.position);
                }
            }
            UpdateQueuePositions();
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        customers.Remove(customer);
        Destroy(customer);
    }
}
