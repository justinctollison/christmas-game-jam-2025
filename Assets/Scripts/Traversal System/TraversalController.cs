using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TraversalController : MonoBehaviour
{
    [Header("Traversal")]
    [SerializeField] private float _traversalDuration = 0.35f;
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private KeyCode _traversalKey = KeyCode.Space;
    [SerializeField]
    private AnimationCurve _jumpCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CharacterController _characterController;
    private TraversalDetector _detector;
    private PlayerController _playerController;

    private bool _isTraversing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _detector = GetComponent<TraversalDetector>();
        _playerController = GetComponent<PlayerController>();
    }

    public bool TryStartTraversal()
    {
        if (_isTraversing)
            return true;

        if (_detector.CurrentTarget == null)
            return false;

        StartCoroutine(PerformTraversal(_detector.CurrentTarget));
        return true;
    }

    private IEnumerator PerformTraversal(TraversalTarget target)
    {
        _playerController.ForceStopVerticalMotion();

        _isTraversing = true;
        _playerController.SetMovementEnabled(false);

        Vector3 startPos = transform.position;
        Vector3 endPos = target.LandingPosition;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = target.LandingRotation;

        float elapsed = 0f;

        _characterController.enabled = false;

        while (elapsed < _traversalDuration)
        {
            float t = elapsed / _traversalDuration;
            float curvedT = _jumpCurve.Evaluate(t);

            Vector3 horizontalPos = Vector3.Lerp(startPos, endPos, t);

            // Vertical jump arc
            float heightOffset = Mathf.Sin(curvedT * Mathf.PI) * _jumpHeight;
            Vector3 finalPos = horizontalPos + Vector3.up * heightOffset;

            transform.position = finalPos;
            //transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        //transform.rotation = endRot;

        _characterController.enabled = true;
        _playerController.SetMovementEnabled(true);
        _isTraversing = false;
    }

    public bool HasTraversalTarget()
    {
        return !_isTraversing && _detector.CurrentTarget != null;
    }

}
