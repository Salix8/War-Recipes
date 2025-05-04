using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform entryPoint;
    public Transform queueStartPoint;
    public int maxQueueSize = 4;
    public float queueSpacing = 1.5f;
    public float spawnInterval = 5f;
    public CustomerButton customerButton;

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
        while (true)
        {
            if (waitingQueue.Count < maxQueueSize)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        Vector3 spawnPos = entryPoint.position;
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            GameObject newCustomer = Instantiate(customerPrefab, hit.position, Quaternion.identity);
            customers.Add(newCustomer);
            waitingQueue.Enqueue(newCustomer);
            Debug.Log("Cliente agregado a la cola: " + newCustomer.name);

            // Mostrar emote default y comenzar espera
            CustomerEmoteController emotes = newCustomer.GetComponent<CustomerEmoteController>();
            if (emotes != null)
            {
                emotes.ShowDefaultEmote();
                emotes.StartWaiting();
            }

            UpdateQueuePositions();
        }
        else
        {
            Debug.LogError("Error al colocar al cliente en el NavMesh.");
        }
    }

    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (GameObject customer in waitingQueue)
        {
            Vector3 queuePosition = queueStartPoint.position + Vector3.forward * (index * queueSpacing);
            NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(queuePosition);
            }
            index++;
        }
    }

public void AssignCustomersToTables()
{
    if (waitingQueue.Count > 0)
    {
        GameObject customer = waitingQueue.Dequeue();
        Customer customerScript = customer.GetComponent<Customer>();
        CustomerEmoteController emotes = customer.GetComponent<CustomerEmoteController>();
        Transform chair = tableManager.GetAvailableChair();

        if (chair != null)
        {
            if (emotes != null)
            {
                emotes.StopWaiting(); // Detiene el contador de espera

                // Espera a que se siente para mostrar el emote "Bien"
                customerScript.OnSeated += () =>
                {
                    emotes.ShowBienEmote();
                };
            }

            customerScript.MoveToSeat(chair);
            UpdateQueuePositions();
        }
        else
        {
            Debug.LogError("No hay sillas disponibles en TableManager.");
        }
    }
    else
    {
        Debug.LogError("No hay clientes en la cola para asignar a las mesas.");
    }
}


    public int GetWaitingQueueCount()
    {
        return waitingQueue.Count;
    }
}
