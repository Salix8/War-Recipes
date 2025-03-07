using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointClickMovement : MonoBehaviour
{
    public Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterInputActions inputActions; // InputAction asset

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        inputActions = new CharacterInputActions(); // Crea una instancia de InputActions
        // inputActions.WASD.Disable(); // Habilita el ActionMap de PointClick
        inputActions.PointClick.Enable(); // Habilita el ActionMap de PointClick
        inputActions.PointClick.ClickPosition.performed += OnClickPosition; // Suscribe al evento de ClickPosition
    }

    void OnDestroy()
    {
        inputActions.PointClick.ClickPosition.performed -= OnClickPosition; // Desuscribe para evitar errores
        inputActions.PointClick.Disable(); // Deshabilita el ActionMap al destruir el objeto
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator no encontrado en el personaje. Asegúrate de que el personaje tenga un componente Animator.");
                enabled = false; // Desactivar el script si no hay Animator
            }
        }
    }

    void Update()
    {
        // Actualizar la animación de caminar en función de la velocidad del NavMeshAgent
        if (navMeshAgent.velocity.magnitude > 0.1f) // Umbral para evitar animaciones en movimiento muy pequeño
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude); // Puedes usar la magnitud de la velocidad como "Speed"
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    void OnClickPosition(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            navMeshAgent.destination = hit.point; // Establece el destino del NavMeshAgent al punto donde hizo clic el ratón.
        }
    }

    public void PlayPickUpAnimation()
    {
        animator.SetTrigger("IsPickingUp");
    }

    // Función para llamar cuando se interactúa con un objeto en la escena A (ejemplo)
    public void InteractWithObject()
    {
        PlayPickUpAnimation();
        Debug.Log("Interactuando con objeto en Escena A");
        // Aquí iría la lógica de interacción con el objeto (ej. servir comida)
    }
}