using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public InputAction MoveAction;
    [SerializeField] protected float m_Speed = 5f;
    [SerializeField] protected float m_TurnSpeed = 180f;

    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;
    protected AudioSource m_AudioSource;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    //private float m_MovementInputValue;
    //private float m_TurnInputValue;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        MoveAction.Enable();
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    //private void OnEnable()
    //{
    //    m_Rigidbody.isKinematic = false;

    //    m_MovementInputValue = 0f;
    //    m_TurnInputValue = 0f;
    //}

    //private void OnDisable()
    //{
    //    m_Rigidbody.isKinematic = true;
    //}

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Speed * Time.deltaTime);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, m_TurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            m_Rigidbody.MoveRotation(m_Rotation);

            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        //Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, m_TurnSpeed * Time.deltaTime, 0f);
        //m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}
