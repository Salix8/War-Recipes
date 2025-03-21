using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections.Generic;

public class CharacterManagerCocina : MonoBehaviour
{
    public static CharacterManagerCocina Instance;
    public Animator animator;
    private PointClickMovement pointClickMovement;
    private List<string> ingredients = new List<string>();

    private Vector3 stationPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        animator = GetComponent<Animator>();
        pointClickMovement = GetComponent<PointClickMovement>();
    }

    public void AddIngredient(string ingredient)
    {
        ingredients.Add(ingredient);
        Debug.Log("Ingrediente recogido: " + ingredient);
    }

    public bool HasIngredients(List<string> requiredIngredients)
    {
        foreach (string ingredient in requiredIngredients)
        {
            if (!ingredients.Contains(ingredient)) return false;
        }
        return true;
    }

    public void StartCooking(Vector3 position)
    {
        pointClickMovement.navMeshAgent.SetDestination(position);
        animator.SetTrigger("IsCooking");
    }

    public void MoveToStation(Transform station)
    {
        if (station != null)
        {
            pointClickMovement.navMeshAgent.SetDestination(station.position);
            animator.SetTrigger("IsCooking");
        }
        else
        {
            Debug.LogError("Station transform is not assigned.");
        }
    }
}