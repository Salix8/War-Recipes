using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerKitchenMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float moveSpeed = 5f;
    public float acceleration = 3f;
    public float deceleration = 5f;
    private float currentSpeed = 0f;

    private CharacterInputActions inputActions;
    private Vector2 moveInput;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Disable(); // Deshabilita el ActionMap WASD
        inputActions.WASD.Enable(); // Habilita el ActionMap WASD

        inputActions.WASD.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled += context => moveInput = Vector2.zero;
    }

    void OnDestroy()
    {
        inputActions.WASD.Move.performed -= context => moveInput = context.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled -= context => moveInput = Vector2.zero;
        inputActions.WASD.Disable(); // Deshabilita el ActionMap al destruir el objeto
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator no encontrado en el personaje en WASDMovement. Asegúrate de que el personaje tenga un componente Animator.");
                enabled = false;
            }
        }
        if (characterController == null)
        {
            Debug.LogError("CharacterController no encontrado en el personaje en WASDMovement. Asegúrate de que el personaje tenga un componente CharacterController.");
            enabled = false;
        }
    }

    void Update()
    {
        bool isMoving = moveInput.magnitude >= 0.1f;

        // Aplicar aceleración o desaceleración progresiva
        if (isMoving)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        if (isMoving)
        {
            // Obtener la dirección de la cámara sin la componente Y
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Convertir el input en movimiento relativo a la cámara
            Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

            // Mover el personaje
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotar el personaje en la dirección del movimiento
            transform.forward = moveDirection;

            // Actualizar animaciones
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {
            // Detener animaciones cuando no hay movimiento
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }
}