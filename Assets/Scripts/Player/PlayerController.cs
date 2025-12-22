using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Movement Settings
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 12f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _gravity = -25f;
    private bool _movementEnabled = true;
    #endregion

    #region Jump Settings
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _groundCheckBuffer = -2f;
    #endregion

    #region References
    [SerializeField] private Animator _animator;
    #endregion

    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Vector3 _velocity;
    private Vector3 _currentMoveVelocity;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_cameraTransform == null && Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleGravity();
        HandleJump();
    }

    #region Core Logic


    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }

    private void HandleMovement()
    {
        if (!_movementEnabled) { return; }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);
        input = Vector3.ClampMagnitude(input, 1f);

        if (input.sqrMagnitude < 0.01f)
        {
            _currentMoveVelocity = Vector3.Lerp(
                _currentMoveVelocity,
                Vector3.zero,
                _acceleration * Time.deltaTime
            );
            return;
        }

        // Camera-relative direction
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * input.z + camRight * input.x;

        // Smooth acceleration
        Vector3 targetVelocity = moveDirection * _moveSpeed;
        _currentMoveVelocity = Vector3.Lerp(
            _currentMoveVelocity,
            targetVelocity,
            _acceleration * Time.deltaTime
        );

        // Rotate player ONLY when moving
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );

        _characterController.Move(_currentMoveVelocity * Time.deltaTime);
        // TODO Handle Walking animation
    }

    private void HandleGravity()
    {
        if (IsGrounded && _velocity.y < 0f)
        {
            _velocity.y = _groundCheckBuffer;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (!IsGrounded) return;

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            // TODO Handle Jump animation
        }
    }

    private void HandleGroundCheck()
    {
        IsGrounded = _characterController.isGrounded;
    }

    #endregion

    #region Future Hooks

    public void EnterStealth()
    {
        // Placeholder
    }

    public void Interact()
    {
        // Placeholder
    }

    #endregion
}
