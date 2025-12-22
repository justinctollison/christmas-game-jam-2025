using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _gravity = -20f;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 1.5f;

    private CharacterController _characterController;
    private TraversalController _traversalController;
    private Transform _cameraTransform;

    private Vector3 _velocity;
    private float _verticalVelocity;
    private bool _isGrounded;
    private bool _movementEnabled = true;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _traversalController = GetComponent<TraversalController>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!_movementEnabled)
            return;

        HandleGroundCheck();
        HandleMovement();
        HandleGravity();
        HandleJump();
    }

    private void HandleGroundCheck()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);

        if (input.sqrMagnitude < 0.01f)
            return;

        // Camera-relative movement (read-only camera)
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * input.z + cameraRight * input.x;

        _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);

        // Rotate ONLY based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    private void HandleGravity()
    {
        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;

        _velocity = Vector3.up * _verticalVelocity;
        _characterController.Move(_velocity * Time.deltaTime);
    }


    private void HandleJump()
    {
        if (!_movementEnabled)
            return;

        if (!Input.GetButtonDown("Jump"))
            return;

        // Give traversal first chance to consume jump
        if (_traversalController != null &&
            _traversalController.TryStartTraversal())
        {
            // Jump input consumed by traversal
            _verticalVelocity = 0f;
            return;
        }

        // Normal jump
        if (_isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(
                _jumpHeight * -2f * _gravity
            );
        }
    }


    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }
}
