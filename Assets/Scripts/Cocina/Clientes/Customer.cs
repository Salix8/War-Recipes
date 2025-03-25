using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private CustomerManager customerManager;

    public void Initialize(CustomerManager manager)
    {
        customerManager = manager;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on customer.");
            return;
        }
    }

    public void MoveTo(Transform destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination.position);
        }
    }

    public void LeaveRestaurant()
    {
        customerManager.RemoveCustomer(gameObject);
    }
}
