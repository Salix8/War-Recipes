using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MovmentPlayer : MonoBehaviour
{
    [Header("Parámetros de movimiento")]
    public float moveSpeed = 5f;
    public float rollSpeedMultiplier = 2f;

    [Header("Animación")]
    public Animator animator;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerAttack playerAttack;

    private CharacterInputActions inputActions;
    private Vector2 moveInput;
    private bool isRolling = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY  | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        inputActions = new CharacterInputActions();
        inputActions.PointClick.Disable();
        inputActions.WASD.Enable();

        inputActions.WASD.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.WASD.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.WASD.Roll.performed += ctx => TryRoll();
        inputActions.WASD.Attack1.performed += ctx => AtaqueBasico();
        inputActions.WASD.Attack2.performed += ctx => AtaqueFuerte();
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }

    void FixedUpdate()
    {
        if (isRolling) return;

        Vector3 direction = GetMoveDirection();

        if (direction.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            transform.forward = direction;

            if (animator)
            {
                animator.SetBool("IsMoving", true);
                animator.SetFloat("Speed", moveInput.magnitude);
            }
        }
        else if (animator)
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        return (camForward * moveInput.y + camRight * moveInput.x).normalized;
    }

    private void TryRoll()
    {
        if (!isRolling && animator != null)
        {
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        isRolling = true;

        animator.SetTrigger("IsRolling");

        // Esperamos un frame para que la animación se active
        yield return null;

        // Obtenemos la duración real de la animación activa
        //float rollAnimDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        Vector3 rollDirection = GetMoveDirection();
        if (rollDirection == Vector3.zero)
            rollDirection = transform.forward;

        float elapsed = 0f;
        float dashSpeed = moveSpeed * rollSpeedMultiplier;

        while (elapsed < 1)
        {
            rb.MovePosition(rb.position + rollDirection * dashSpeed * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(RollCooldownRoutine());
        isRolling = false;
    }

    IEnumerator RollCooldownRoutine()
    {
        float cooldown = 3f;
        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            HUDManager.Instance.UpdateRollCooldown(1f - (elapsed / cooldown));
            elapsed += Time.deltaTime;
            yield return null;
        }

        HUDManager.Instance.UpdateRollCooldown(0f); // Listo
    }


    private void AtaqueBasico()
    {
        if (animator)
            animator.SetTrigger("IsAttacking1");

        if (playerAttack != null) 
            playerAttack.RealizarAtaque1();

        StartCoroutine(Attack1CooldownRoutine());
    }

    private void AtaqueFuerte()
    {
        if (animator)
            animator.SetTrigger("IsAttacking2");

        if (playerAttack != null)
            playerAttack.RealizarAtaque2();
        
        StartCoroutine(Attack2CooldownRoutine());
    }

    IEnumerator Attack1CooldownRoutine()
    {
        float cooldownAttack1 = 1.5f; // o el tiempo que quieras
        float elapsed = 0f;

        while (elapsed < cooldownAttack1)
        {
            HUDManager.Instance.UpdateAttack1Cooldown(1f - (elapsed / cooldownAttack1));
            elapsed += Time.deltaTime;
            yield return null;
        }

        HUDManager.Instance.UpdateAttack1Cooldown(0f);
    }

    IEnumerator Attack2CooldownRoutine()
    {
        float cooldownAttack2 = 3f; // o el tiempo que quieras
        float elapsed = 0f;

        while (elapsed < cooldownAttack2)
        {
            HUDManager.Instance.UpdateAttack2Cooldown(1f - (elapsed / cooldownAttack2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        HUDManager.Instance.UpdateAttack2Cooldown(0f);
    }

}
