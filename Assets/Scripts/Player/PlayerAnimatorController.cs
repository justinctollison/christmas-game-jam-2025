using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Transform _modelRoot;
    [SerializeField] private int turnLayerIndex = 1; // Index of additive Turn layer in Animator

    private Animator _animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int TurnHash = Animator.StringToHash("Turn");
    private static readonly int GroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private float _currentTurn;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void UpdateMovement(Vector3 desiredDir, float slowdown)
    {
        if (desiredDir.sqrMagnitude < 0.001f)
        {
            _animator.SetFloat(SpeedHash, 0f, 0.1f, Time.deltaTime);
            _animator.SetFloat(TurnHash, 0f, 0.15f, Time.deltaTime);
            _animator.SetLayerWeight(turnLayerIndex, 0f);
            return;
        }

        Vector3 forward = _modelRoot.forward;
        float signedAngle = Vector3.SignedAngle(forward, desiredDir, Vector3.up);

        // Clamp Turn for additive layer to ±90° so lean doesn't go backwards
        float turnAngleForAnimation = Mathf.Clamp(signedAngle, -90f, 90f);
        float targetTurn = turnAngleForAnimation / 90f;

        // Smooth Turn blending
        _currentTurn = Mathf.Lerp(_currentTurn, targetTurn, Time.deltaTime * 6f);

        // Optional: Predictive lean (slightly leads rotation)
        float predictedTurn = targetTurn * 1.2f;
        _currentTurn = Mathf.Lerp(_currentTurn, predictedTurn, Time.deltaTime * 8f);

        // Scale additive Turn layer weight by angular velocity
        float leanMultiplier = Mathf.Clamp01(Mathf.Abs(signedAngle) / 45f);
        _animator.SetLayerWeight(turnLayerIndex, leanMultiplier);

        // Determine forward/backward direction for SpeedHash
        float forwardDot = Vector3.Dot(forward, desiredDir);
        float speedParam = slowdown * Mathf.Sign(forwardDot); // negative if walking backward

        // Update Animator
        _animator.SetFloat(SpeedHash, speedParam, 0.1f, Time.deltaTime);
        _animator.SetFloat(TurnHash, _currentTurn, 0.1f, Time.deltaTime);

        // Keep additive turn animation at normal speed
        _animator.speed = 1.0f;
    }

    public void SetGrounded(bool grounded)
    {
        _animator.SetBool(GroundedHash, grounded);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(JumpHash);
    }
}
