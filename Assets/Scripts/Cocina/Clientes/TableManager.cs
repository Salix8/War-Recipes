using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public List<Transform> tables; // Lista de mesas en el restaurante

    private Queue<Transform> availableTables = new Queue<Transform>();

    void Start()
    {
        foreach (Transform table in tables)
        {
            availableTables.Enqueue(table);
        }
    }

    public Transform GetAvailableTable()
    {
        if (availableTables.Count > 0)
        {
            return availableTables.Dequeue();
        }
        return null;
    }

    public void ReleaseTable(Transform table)
    {
        availableTables.Enqueue(table);
    }
}