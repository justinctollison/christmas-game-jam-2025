using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Tooltip("Angle (deg) where forward speed reaches zero")]
    [SerializeField] private float _turnSlowdownAngle = 60f;

    [Tooltip("Max angular turn rate at low speed (deg/sec)")]
    [SerializeField] private float _maxTurnRate = 140f;

    [Tooltip("Min angular turn rate at high speed (deg/sec)")]
    [SerializeField] private float _minTurnRate = 70f;

    [SerializeField] private float _gravity = -20f;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 1.5f;

    private CharacterController _characterController;
    private TraversalController _traversalController;
    private PlayerAnimatorController _animatorController;
    private Transform _cameraTransform;

    private float _verticalVelocity;
    private bool _isGrounded;
    private bool _movementEnabled = true;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _traversalController = GetComponent<TraversalController>();
        _animatorController = GetComponent<PlayerAnimatorController>();
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

        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _animatorController.SetGrounded(_isGrounded);
    }

    // --------------------------------------------------
    // ANIMAL LOCOMOTION CORE
    // --------------------------------------------------
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);

        // No input → idle
        if (input.sqrMagnitude < 0.001f)
        {
            _animatorController.UpdateMovement(Vector3.zero, 0f);
            return;
        }

        // Camera-relative desired direction
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredDir =
            (camForward * input.z + camRight * input.x).normalized;

        // ---------------------------------------------
        // TURN SEVERITY → SLOWDOWN
        // ---------------------------------------------
        float signedAngle = Vector3.SignedAngle(
            transform.forward,
            desiredDir,
            Vector3.up
        );

        float turnMagnitude = Mathf.Abs(signedAngle);

        float slowdown = Mathf.Clamp01(
            1f - (turnMagnitude / _turnSlowdownAngle)
        );

        // ---------------------------------------------
        // TRUE CIRCULAR TURNING (ANGULAR VELOCITY)
        // ---------------------------------------------
        float turnRate = Mathf.Lerp(
            _maxTurnRate,   // sharp turns
            _minTurnRate,   // wide arcs
            slowdown
        );

        float maxStep = turnRate * Time.deltaTime;
        float turnStep = Mathf.Clamp(signedAngle, -maxStep, maxStep);

        transform.Rotate(0f, turnStep, 0f);

        // ---------------------------------------------
        // FORWARD MOVEMENT (NO STRAFING)
        // ---------------------------------------------
        Vector3 forwardMove =
            transform.forward * (_moveSpeed * slowdown * Time.deltaTime);

        _characterController.Move(forwardMove);

        // ---------------------------------------------
        // ANIMATOR (DRIVEN BY SAME DATA)
        // ---------------------------------------------
        _animatorController.UpdateMovement(desiredDir, slowdown);
    }

    private void HandleGravity()
    {
        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;

        _characterController.Move(
            Vector3.up * _verticalVelocity * Time.deltaTime
        );
    }

    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        _animatorController.TriggerJump();

        if (_traversalController != null &&
            _traversalController.TryStartTraversal())
        {
            _verticalVelocity = 0f;
            return;
        }

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
