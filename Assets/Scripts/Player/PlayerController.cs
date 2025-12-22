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
    private Transform _cameraTransform;

    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _movementEnabled = true;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!_movementEnabled)
            return;

        HandleGroundCheck();
        HandleMovement();
        HandleGravity();
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
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }
}
