using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    public float movementSpeed = 3;
    public Camera playerCamera;
    
    private CharacterController m_controller;
    private Animator m_Animator;
    private Vector3 m_movementInput;
    private float m_TurnAmount;
    private float m_ForwardAmount;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Rotate();
        Move();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", -m_TurnAmount, 0.1f, Time.deltaTime);
    }

    private void Rotate()
    {
        var direction = Vector3.RotateTowards(transform.forward, m_movementInput.normalized, 3 * Time.deltaTime, 0.0f);
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Move()
    {
        // m_controller.Move(m_movementInput * (movementSpeed * Time.deltaTime));
    }
    
    private void OnMove(InputValue value)
    {
        Vector2 newInputMovement = value.Get<Vector2>();
        m_movementInput = new Vector3(newInputMovement.x, 0.0f, newInputMovement.y);
        m_movementInput = Quaternion.Euler(0.0f, playerCamera.transform.eulerAngles.y, 0.0f) * m_movementInput;
        if (m_movementInput.magnitude > 1f) 
            m_movementInput.Normalize();
        m_TurnAmount = Vector3.SignedAngle(m_movementInput, transform.forward, Vector3.up) / 180;
        m_ForwardAmount = Mathf.Abs(m_movementInput.x) + Mathf.Abs(m_movementInput.z);
    }
}
