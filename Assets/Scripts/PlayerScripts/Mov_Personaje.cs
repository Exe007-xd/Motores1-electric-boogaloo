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

    [Header("Sonidos")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;

    private CharacterController _controller;

    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;
    [SerializeField] Animator animator;

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
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
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

        bool isGrounded = _controller.isGrounded;

        if (isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 moveDirection = forward * _move.y + right * _move.x;

        // Animaciones
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool("IsWalking", isMoving);
        animator.SetBool("IsRunning", isMoving && _speed == _sprintSpeed);

        Vector3 velocity = moveDirection * _speed + Vector3.up * _verticalVelocity;

        _controller.Move(velocity * Time.deltaTime);

        if (_shouldFaceMoveDirection && isMoving)
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
            animator.SetBool("IsRunning", true);
        }
        else if (context.canceled)
        {
            _speed = _normalSpeed;
            animator.SetBool("IsRunning", false);
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

    public void PlayFootstep()
    {
        if (!_controller.isGrounded)
            return;

        if (footstepSource == null || footstepClips.Length == 0)
            return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepSource.PlayOneShot(clip);
    }
}