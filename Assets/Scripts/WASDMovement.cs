using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WASDMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float moveSpeed = 5f;
    public float rollSpeedMultiplier = 2f;
    public float rollDuration = 0.5f;
    public float attackingIdleDuration = 1f;

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

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized; // Normalizar para evitar movimiento más rápido en diagonal
        if (moveDirection.magnitude >= 0.1f) // Umbral para evitar movimiento muy pequeño
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", moveDirection.magnitude); // Puedes usar la magnitud como "Speed"
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            transform.forward = moveDirection; // Rotar al personaje en la dirección del movimiento
        }
        else
        {
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
        Vector3 rollDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (rollDirection == Vector3.zero) rollDirection = transform.forward; // Si no hay input de movimiento, rodar hacia adelante

        while (rollTimer < rollDuration)
        {
            characterController.Move(rollDirection * moveSpeed * rollSpeedMultiplier * Time.deltaTime);
            rollTimer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

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