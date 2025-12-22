using UnityEngine;

public class TraversalDetector : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float _forwardDistance = 2.0f;
    [SerializeField] private float _upwardDistance = 2.5f;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private LayerMask _traversalLayer;

    [Header("Intent")]
    [SerializeField] private float _minFacingDot = 0.5f;

    private TraversalTarget _currentTarget;

    public TraversalTarget CurrentTarget => _currentTarget;

    private void Update()
    {
        DetectTraversalTarget();
    }

    private void DetectTraversalTarget()
    {
        _currentTarget = null;

        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Vector3 forwardDir = transform.forward;

        // 1?? Forward probe (horizontal traversal)
        TryDetect(origin, forwardDir, _forwardDistance);

        if (_currentTarget != null)
            return;

        // 2?? Angled-up probe (vertical traversal)
        Vector3 upwardDir = (forwardDir + Vector3.up * 0.75f).normalized;
        TryDetect(origin, upwardDir, _upwardDistance);
    }

    private void TryDetect(Vector3 origin, Vector3 direction, float distance)
    {
        if (Physics.SphereCast(origin, _sphereRadius, direction, out RaycastHit hit, distance, _traversalLayer))
        {
            TraversalTarget target = hit.collider.GetComponentInParent<TraversalTarget>();
            if (target == null)
                return;

            Vector3 toTarget = (target.LandingPosition - transform.position).normalized;
            float facingDot = Vector3.Dot(transform.forward, toTarget);

            if (facingDot < _minFacingDot)
                return;

            _currentTarget = target;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + transform.forward * _forwardDistance);

        Gizmos.color = Color.cyan;
        Vector3 upwardDir = (transform.forward + Vector3.up * 0.75f).normalized;
        Gizmos.DrawLine(origin, origin + upwardDir * _upwardDistance);
    }
}
