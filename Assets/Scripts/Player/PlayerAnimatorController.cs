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
    private static readonly int TraversalJumpHash =
        Animator.StringToHash("IsTraversalJump");

    private float _currentTurn;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TriggerJump(bool isTraversal)
    {
        _animator.SetBool(TraversalJumpHash, isTraversal);
        _animator.SetTrigger(JumpHash);
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

        // Signed angle between where we face and where we want to go
        float signedAngle = Vector3.SignedAngle(forward, desiredDir, Vector3.up);

        // Clamp for additive lean (animals never lean fully backwards)
        float clampedAngle = Mathf.Clamp(signedAngle, -90f, 90f);
        float targetTurn = clampedAngle / 90f;

        // Immediate kick-in so lean starts instantly
        if (Mathf.Abs(signedAngle) > 3f)
            _currentTurn += Mathf.Sign(signedAngle) * 0.05f;

        // Smooth turn blending
        _currentTurn = Mathf.Lerp(_currentTurn, targetTurn, Time.deltaTime * 6f);

        // Predictive lean (upper body leads rotation slightly)
        float predictedTurn = targetTurn * 1.2f;
        _currentTurn = Mathf.Lerp(_currentTurn, predictedTurn, Time.deltaTime * 8f);

        // Clamp to blend tree range (-0.5 to 0.5)
        float turnParam = Mathf.Clamp(_currentTurn, -0.5f, 0.5f);

        // Scale additive layer weight by angular intent
        float leanWeight = Mathf.Clamp01(Mathf.Abs(signedAngle) / 45f);
        _animator.SetLayerWeight(turnLayerIndex, leanWeight);

        // Forward / backward movement
        float forwardDot = Vector3.Dot(forward, desiredDir);
        float speedParam = slowdown * Mathf.Sign(forwardDot);

        // Animator params
        _animator.SetFloat(SpeedHash, speedParam, 0.1f, Time.deltaTime);
        _animator.SetFloat(TurnHash, turnParam, 0.1f, Time.deltaTime);

        // Always keep animation speed normal
        _animator.speed = 1f;
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
