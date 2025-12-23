using UnityEngine;

public class TraversalDetector : MonoBehaviour
{
    [Header("General Detection")]
    [SerializeField] private float _forwardDistance = 2.5f;
    [SerializeField] private float _sphereRadius = 0.4f;
    [SerializeField, Range(0f, 1f)] private float _minFacingDot = 0.65f;
    [SerializeField] private LayerMask _traversalLayerMask;

    [Header("Vertical Traversal (Angle Range)")]
    [SerializeField, Range(0f, -90f)] private float _minUpwardAngle = 35f;
    [SerializeField, Range(0f, -90f)] private float _maxUpwardAngle = 60f;
    [SerializeField] private float _angleStep = 5f;
    [SerializeField] private float _verticalDetectionDistance = 2.5f;

    private TraversalTarget _previousTarget;

    public TraversalTarget CurrentTarget { get; private set; }

    private void Update()
    {
        DetectTraversalTarget();
    }

    private void DetectTraversalTarget()
    {
        CurrentTarget = null;

        Vector3 origin = transform.position + Vector3.up;

        // -------------------------
        // Horizontal Traversal
        // -------------------------
        if (Physics.SphereCast(
            origin,
            _sphereRadius,
            transform.forward,
            out RaycastHit horizontalHit,
            _forwardDistance,
            _traversalLayerMask))
        {
            if (horizontalHit.collider.TryGetComponent(out TraversalTarget target))
            {
                if (IsFacingTarget(target))
                {
                    CurrentTarget = target;
                    return;
                }
            }
        }

        // -------------------------
        // Vertical Traversal (Angle Cone)
        // -------------------------
        for (float angle = _minUpwardAngle; angle <= _maxUpwardAngle; angle += _angleStep)
        {
            Vector3 direction =
                Quaternion.AngleAxis(angle, transform.right) * transform.forward;

            if (Physics.SphereCast(
                origin,
                _sphereRadius,
                direction,
                out RaycastHit hit,
                _verticalDetectionDistance,
                _traversalLayerMask))
            {
                if (hit.collider.TryGetComponent(out TraversalTarget target))
                {
                    if (IsFacingTarget(target))
                    {
                        CurrentTarget = target;
                        return;
                    }
                }
            }
        }
    }

    private bool IsFacingTarget(TraversalTarget target)
    {
        Vector3 toTarget =
            (target.LandingPosition - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, toTarget);
        return dot >= _minFacingDot;
    }

    // -------------------------
    // Debug
    // -------------------------
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up;

        // Horizontal
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            origin + transform.forward * _forwardDistance,
            _sphereRadius
        );

        // Vertical cone
        Gizmos.color = Color.cyan;
        for (float angle = _minUpwardAngle; angle <= _maxUpwardAngle; angle += _angleStep)
        {
            Vector3 dir =
                Quaternion.AngleAxis(angle, transform.right) * transform.forward;
            Gizmos.DrawRay(origin, dir * _verticalDetectionDistance);
        }

        // Active target
        if (CurrentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                transform.position,
                CurrentTarget.LandingPosition
            );
        }
    }
}
