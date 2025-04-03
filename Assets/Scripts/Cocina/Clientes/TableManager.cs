using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public List<Transform> chairs;
    public List<Transform> tableObjects; // Objetos en la mesa que los clientes mirarán

    private Queue<Transform> availableChairs = new Queue<Transform>();
    private Dictionary<Transform, Transform> chairToTableObject = new Dictionary<Transform, Transform>();

    void Start()
    {
        if (chairs.Count != tableObjects.Count)
        {
            Debug.LogWarning("El número de sillas y objetos en la mesa no coincide.");
        }

        for (int i = 0; i < chairs.Count; i++)
        {
            availableChairs.Enqueue(chairs[i]);

            if (i < tableObjects.Count)
            {
                chairToTableObject[chairs[i]] = tableObjects[i];
            }
            else
            {
                Debug.LogWarning($"No se encontró un objeto de mesa para la silla {chairs[i].name}");
            }
        }
    }

    public Transform GetAvailableChair()
    {
        if (availableChairs.Count > 0)
        {
            return availableChairs.Dequeue();
        }
        return null;
    }

    public void ReleaseChair(Transform chair)
    {
        availableChairs.Enqueue(chair);
    }

    public bool HasAvailableChairs()
    {
        return availableChairs.Count > 0;
    }

public Transform GetTableForChair(Transform chair)
{
    if (chairToTableObject.TryGetValue(chair, out Transform tableObject))
    {
        return tableObject;
    }

    Debug.LogWarning($"No se encontró un objeto de mesa para la silla {chair.name}");
    return null;
}


}
