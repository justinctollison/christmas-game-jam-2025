using UnityEngine;

public class TraversalTarget : MonoBehaviour
{
    [Header("Traversal Settings")]
    [SerializeField] private Transform _landingPoint;
    [SerializeField] private bool _overrideFacing = true;

    public Vector3 LandingPosition =>
        _landingPoint != null ? _landingPoint.position : transform.position;

    public Quaternion LandingRotation =>
        _overrideFacing ? transform.rotation : Quaternion.identity;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(LandingPosition, 0.15f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, LandingPosition);
    }
}
