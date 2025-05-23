using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointClickMovement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    private CharacterInputActions inputActions;

    [SerializeField] private LayerMask groundLayer; // Capa del suelo
    [SerializeField] private LayerMask interactableLayer; // Capa para objetos interactuables

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Enable();
    }

void OnDisable()
{
    if (inputActions != null)
        inputActions.PointClick.Disable();
}

void OnDestroy()
{
    if (inputActions != null)
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

        // Solución de "mirar hacia un punto bugueado" si está atascado
    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
    {
        // Si el agente no se está moviendo y ha llegado, asegúrate de que no siga girando
        if (navMeshAgent.velocity.sqrMagnitude == 0f)
        {
            navMeshAgent.ResetPath(); // Detiene todo movimiento residual
        }
    }

    }

void ProcessClick()
{
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    RaycastHit hit;

    // 1️ Interactuar si se clicó un objeto interactuable
    if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
    {
        InteractWithObject(hit.collider.gameObject);
        return;
    }

    // Cancelar destino anterior para evitar arrastre de rotación o atasco
    if (navMeshAgent.hasPath)
    {
        navMeshAgent.ResetPath();
    }

    // 2️ Movimiento si se clicó en el suelo
    if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
    {
        NavMeshHit navHit;

        // Tolerancia ajustable: 1.0f puede ser muy grande si tienes zonas estrechas
        float maxDistanceToSample = 0.5f;

        if (NavMesh.SamplePosition(hit.point, out navHit, maxDistanceToSample, NavMesh.AllAreas))
        {
            
            NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(navHit.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(navHit.position);
        }
        else
        {
            Debug.Log("Destino no alcanzable. Ruta incompleta.");
        }

            // Seguridad adicional: verifica que el agente esté en el NavMesh
                if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
                {
                    navMeshAgent.SetDestination(navHit.position);
                }
                else
                {
                    Debug.LogWarning("El NavMeshAgent no está en una zona válida del NavMesh.");
                }
        }
        else
        {
            Debug.Log("No se encontró una posición válida en el NavMesh cerca del punto clicado.");
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
