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

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new CharacterInputActions();
        inputActions.PointClick.Disable();
        inputActions.WASD.Enable();

        inputActions.WASD.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled += context => moveInput = Vector2.zero;
        inputActions.WASD.Roll.performed += context => Roll();
        inputActions.WASD.Attack1.performed += context => Attack1();
        inputActions.WASD.Attack2.performed += context => Attack2();
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

            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", currentSpeed);
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
        float elapsedTime = 0f;
        Vector3 rollDirection = transform.forward;

        while (elapsedTime < rollDuration)
        {
            characterController.Move(rollDirection * moveSpeed * rollSpeedMultiplier * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    void Attack1()
    {
        if (isRolling || isAttacking) return;
        attackRoutine = StartCoroutine(AttackRoutine("IsAttacking1"));
    }

    void Attack2()
    {
        if (isRolling || isAttacking) return;
        attackRoutine = StartCoroutine(AttackRoutine("IsAttacking2"));
    }

    IEnumerator AttackRoutine(string attackTrigger)
    {
        isAttacking = true;
        animator.SetTrigger(attackTrigger);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 0.9f);

        isAttacking = false;

        bool isMoving = moveInput.magnitude > 0.1f;
        if (isMoving)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }
}
