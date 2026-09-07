using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class Mov_Personaje : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _normalSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 10f;

    [Header("Salto y Gravedad")]
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpHeight = 3f;

    [Header("Camara")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private bool _shouldFaceMoveDirection = false;



    private CharacterController _controller;

    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        _speed = _normalSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _move.y + right * _move.x;
        _controller.Move(moveDirection * _speed * Time.deltaTime);

        if (_shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }

    }

    
     
    

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _speed = _sprintSpeed;
        }
        else if (context.canceled)
        {
            _speed = _normalSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _controller.isGrounded)
        {
            _verticalVelocity =
                Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}