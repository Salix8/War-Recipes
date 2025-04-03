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
            UpdateQueuePositions();
        }
        else
        {
            Debug.LogError("Failed to place customer on NavMesh.");
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

    public void AssignCustomersToTables()
    {
        if (waitingQueue.Count > 0)
        {
            Transform chair = tableManager.GetAvailableChair();
            if (chair != null)
            {
                GameObject customer = waitingQueue.Dequeue();
                Customer customerScript = customer.GetComponent<Customer>();
                customerScript.MoveToSeat(chair);
                UpdateQueuePositions();
            }
        }
    }
}
