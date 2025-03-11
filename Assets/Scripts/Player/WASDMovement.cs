using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WASDMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float rollSpeedMultiplier = 2f;
    public float rollDuration = 0.5f;
    public float attackingIdleDuration = 1f;
    public float moveSpeed = 5f;
    public float acceleration = 3f;
    public float deceleration = 5f;
    private float currentSpeed = 0f;

    private CharacterInputActions inputActions;
    private Vector2 moveInput;
    private bool isRolling = false;
    private Coroutine attackingIdleCoroutine;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Disable(); // Deshabilita el ActionMap WASD
        inputActions.WASD.Enable(); // Habilita el ActionMap WASD

        inputActions.WASD.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled += context => moveInput = Vector2.zero;
        inputActions.WASD.Roll.performed += context => Roll();
        inputActions.WASD.Attack1.performed += context => Attack1();
        inputActions.WASD.Attack2.performed += context => Attack2();
    }

    void OnDestroy()
    {
        inputActions.WASD.Move.performed -= context => moveInput = context.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled -= context => moveInput = Vector2.zero;
        inputActions.WASD.Roll.performed -= context => Roll();
        inputActions.WASD.Attack1.performed -= context => Attack1();
        inputActions.WASD.Attack2.performed -= context => Attack2();
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
        if (isRolling) return; // No mover si está rodando

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


        if (moveInput.magnitude >= 0.1f) // Si hay input, calcular dirección
        {
            // Obtener la dirección de la cámara sin la componente Y
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0; // Asegurarse de que el personaje no se incline
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


    void Roll()
    {
        if (isRolling) return;
        isRolling = true;
        animator.SetTrigger("IsRolling");
        StartCoroutine(RollCoroutine());
    }

    IEnumerator RollCoroutine()
    {
        float rollTimer = 0f;

        // Obtener la dirección de la cámara sin la componente Y
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convertir el input en movimiento relativo a la cámara
        Vector3 rollDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // Si no hay input de movimiento, rodar hacia adelante según la orientación del personaje
        if (rollDirection == Vector3.zero)
            rollDirection = transform.forward;

        // Aplicar movimiento de rodar
        while (rollTimer < rollDuration)
        {
            characterController.Move(rollDirection * moveSpeed * rollSpeedMultiplier * Time.deltaTime);
            rollTimer += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
    }
    /*IEnumerator RollCoroutine()
    {
        isRolling = true;

        // Iniciar la animación del roll
        animator.CrossFade("Roll", 0.1f); 

        // Esperar un frame para asegurarnos de que la animación ha comenzado
        yield return null;

        // Obtener la duración de la animación de roll
        float rollAnimDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Obtener la dirección de la cámara sin la componente Y
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convertir el input en movimiento relativo a la cámara
        Vector3 rollDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // Si no hay input de movimiento, rodar hacia adelante según la orientación del personaje
        if (rollDirection == Vector3.zero)
            rollDirection = transform.forward;

        // Mover al personaje durante la duración de la animación de roll
        float rollSpeed = moveSpeed * rollSpeedMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < rollAnimDuration)
        {
            characterController.Move(rollDirection * rollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaurar estado después del roll
        isRolling = false;

        // Asegurar que la animación de movimiento se actualiza correctamente
        if (moveInput.magnitude >= 0.1f)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", moveInput.magnitude);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }
*/


    void Attack1()
    {
        animator.SetTrigger("IsAttacking1");
        StartAttackingIdleTimer();
    }

    void Attack2()
    {
        animator.SetTrigger("IsAttacking2");
        StartAttackingIdleTimer();
    }

    void StartAttackingIdleTimer()
    {
        if (attackingIdleCoroutine != null)
        {
            StopCoroutine(attackingIdleCoroutine); // Detener corrutina previa si existe
        }
        attackingIdleCoroutine = StartCoroutine(AttackingIdleCoroutine());
    }

    IEnumerator AttackingIdleCoroutine()
    {
        yield return new WaitForSeconds(attackingIdleDuration);
        animator.SetTrigger("Attacking_Idle"); // Trigger para la animación Attacking_Idle después del tiempo
    }

    public void RecieveHit()
    {
        int randomHit = Random.Range(1, 3); // 1 o 2 para RecieveHit o RecieveHit_2
        if (randomHit == 1)
        {
            animator.SetTrigger("IsHit"); // Trigger para RecieveHit
        }
        else
        {
            animator.SetTrigger("IsHit"); // Trigger para RecieveHit_2 (mismo trigger, Animator decidirá la animación)
        }
    }

    public void Die()
    {
        animator.SetTrigger("IsDead");
    }
}