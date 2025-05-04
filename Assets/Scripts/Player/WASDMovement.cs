using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WASDMovementImproved : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float rollSpeedMultiplier = 2f;
    public float rollDuration = 0.5f;
    public float moveSpeed = 5f;
    public float acceleration = 3f;
    public float deceleration = 5f;
    public float rotationSpeed = 720f;
    private float currentSpeed = 0f;

    private CharacterInputActions inputActions;
    private Vector2 moveInput;
    private bool isRolling = false;
    private bool isAttacking = false;
    private Coroutine attackRoutine;

    [Header("Attack Settings")]
public float attackPauseDuration = 0.9f;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Disable();
        inputActions.WASD.Enable();

        inputActions.WASD.Move.performed += OnMovePerformed;
        inputActions.WASD.Move.canceled += OnMoveCanceled;
        inputActions.WASD.Roll.performed += OnRollPerformed;
        inputActions.WASD.Attack1.performed += OnAttack1Performed;
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.WASD.Move.performed -= OnMovePerformed;
            inputActions.WASD.Move.canceled -= OnMoveCanceled;
            inputActions.WASD.Roll.performed -= OnRollPerformed;
            inputActions.WASD.Attack1.performed -= OnAttack1Performed;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        Roll();
    }

    private void OnAttack1Performed(InputAction.CallbackContext context)
    {
        Attack1();
    }

    void Update()
    {
        if (isRolling || isAttacking) return;

        bool isMoving = moveInput.magnitude > 0.1f;
        currentSpeed = Mathf.Lerp(currentSpeed, isMoving ? moveSpeed : 0f, (isMoving ? acceleration : deceleration) * Time.deltaTime);

        if (isMoving)
        {
            Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            Vector3 moveDirection = Camera.main.transform.TransformDirection(direction);
            moveDirection.y = 0;
            moveDirection.Normalize();

            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (animator != null)
            {
                animator.SetBool("IsMoving", true);
                animator.SetFloat("Speed", currentSpeed);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsMoving", false);
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    void Roll()
    {
        if (isRolling) return;
        isRolling = true;

        if (animator != null && AnimatorHasParameter("IsRolling"))
        {
            animator.SetBool("IsRolling", true);
        }
        else
        {
            Debug.LogWarning("Parámetro 'IsRolling' no encontrado en el Animator.");
        }

        StartCoroutine(RollCoroutine());
    }

IEnumerator RollCoroutine()
{
    float elapsedTime = 0f;
    Vector3 rollDirection = transform.forward;

    // Esperar 1 frame para asegurar que la animación arranque visualmente
    yield return null;

    while (elapsedTime < rollDuration)
    {
        characterController.Move(rollDirection * moveSpeed * rollSpeedMultiplier * Time.deltaTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    if (animator != null && AnimatorHasParameter("IsRolling"))
    {
        animator.SetBool("IsRolling", false);
    }

    isRolling = false;
}


    void Attack1()
    {
        if (isRolling || isAttacking) return;
        attackRoutine = StartCoroutine(AttackRoutine("IsAttacking1"));
    }

IEnumerator AttackRoutine(string attackTrigger)
{
    isAttacking = true;

    if (animator != null && AnimatorHasParameter(attackTrigger))
    {
        animator.SetTrigger(attackTrigger);
    }

    yield return new WaitForSeconds(attackPauseDuration); // configurable

    isAttacking = false;

    bool isMoving = moveInput.magnitude > 0.1f;
    if (animator != null)
    {
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Speed", isMoving ? moveSpeed : 0f);
    }
}


    bool AnimatorHasParameter(string paramName)
    {
        if (animator == null) return false;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}
