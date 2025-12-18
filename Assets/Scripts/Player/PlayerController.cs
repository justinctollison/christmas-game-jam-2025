using UnityEngine;

/// <summary>
/// Handles third-person, over-the-shoulder player movement using CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region References
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Animator _animator;
    #endregion

    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    #endregion

    #region Jump & Gravity
    [Header("Jump & Gravity")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundedCheckDistance = 0.2f;
    #endregion

    #region Ground Check
    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private CharacterController _controller;

    private Vector3 _velocity;
    private Vector3 _currentMoveVelocity;
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        if (_cameraTransform == null && Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateAnimator();
    }

    #region Core Logic

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        if (inputDirection.sqrMagnitude < 0.01f)
        {
            _currentMoveVelocity = Vector3.Lerp(
                _currentMoveVelocity,
                Vector3.zero,
                _acceleration * Time.deltaTime
            );
            return;
        }

        // Camera-relative movement
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

        float targetSpeed = _moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed *= _sprintMultiplier;
        }

        Vector3 targetVelocity = moveDirection * targetSpeed;

        _currentMoveVelocity = Vector3.Lerp(
            _currentMoveVelocity,
            targetVelocity,
            _acceleration * Time.deltaTime
        );

        _controller.Move(_currentMoveVelocity * Time.deltaTime);

        // Smooth rotation toward movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    private void HandleJump()
    {
        if (!_isGrounded) return;

        if (_velocity.y < 0f)
        {
            _velocity.y = -2f; // Small downward force to stay grounded
        }

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    private void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void HandleGroundCheck()
    {
        if (_groundCheck == null)
        {
            _isGrounded = _controller.isGrounded;
            return;
        }

        _isGrounded = Physics.CheckSphere(
            _groundCheck.position,
            _groundedCheckDistance,
            _groundLayer
        );
    }

    #endregion

    #region Animation

    private void UpdateAnimator()
    {
        if (_animator == null) return;

        bool isMoving = _currentMoveVelocity.magnitude > 0.1f;

        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetBool("isMoving", isMoving);
        _animator.SetFloat("verticalVelocity", _velocity.y);
    }

    #endregion

    #region Future Extensions

    // Placeholder for stealth mechanics (light/shadow detection)
    public void SetStealthState(bool isHidden)
    {
        // TODO: Implement stealth behavior
    }

    // Placeholder for interaction system (bells, objects, etc.)
    public void Interact()
    {
        // TODO: Implement interaction logic
    }

    #endregion
}
