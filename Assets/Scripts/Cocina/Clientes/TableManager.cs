using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public List<Transform> chairs; // Lista de todas las sillas disponibles

    private Queue<Transform> availableChairs = new Queue<Transform>();

    void Start()
    {
        foreach (Transform chair in chairs)
        {
            availableChairs.Enqueue(chair);
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
}
