using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointClickMovement : MonoBehaviour
{
    public Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterInputActions inputActions;

    [SerializeField] private LayerMask groundLayer; // Capa del suelo
    [SerializeField] private LayerMask interactableLayer; // Capa para objetos interactuables

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Enable();
    }

    void OnDestroy()
    {
        inputActions.PointClick.Disable();
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator no encontrado en el personaje.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        // Solo procesamos si el usuario hizo clic realmente
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcessClick();
        }

        // Actualizar animación según la velocidad del NavMeshAgent
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    void ProcessClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 1️ Verificar si clicamos en un objeto interactuable
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            InteractWithObject(hit.collider.gameObject);
            return; // Evitar que el personaje se mueva si clicamos en un objeto
        }

        // 2️ Verificar si clicamos en el suelo
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(navHit.position);
            }
        }
    }

    void InteractWithObject(GameObject obj)
    {
        animator.SetTrigger("IsPickingUp");
        Debug.Log("Interactuando con objeto: " + obj.name);
        // Aquí puedes agregar más lógica para recoger/interactuar con el objeto
    }
}
