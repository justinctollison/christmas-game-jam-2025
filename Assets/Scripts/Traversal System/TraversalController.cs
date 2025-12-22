using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TraversalController : MonoBehaviour
{
    [Header("Traversal")]
    [SerializeField] private float _traversalDuration = 0.25f;
    [SerializeField] private KeyCode _traversalKey = KeyCode.Space;

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

    private void Update()
    {
        if (_isTraversing)
            return;

        if (_detector.CurrentTarget != null &&
            Input.GetKeyDown(_traversalKey))
        {
            StartCoroutine(PerformTraversal(_detector.CurrentTarget));
        }
    }

    private IEnumerator PerformTraversal(TraversalTarget target)
    {
        transform.LookAt(target.transform);

        _isTraversing = true;
        _playerController.SetMovementEnabled(false);

        Vector3 startPos = transform.position;
        Vector3 endPos = target.LandingPosition;

        Quaternion startRot = transform.rotation;
        //Quaternion endRot = target.LandingRotation;

        float elapsed = 0f;

        while (elapsed < _traversalDuration)
        {
            float t = elapsed / _traversalDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            Vector3 nextPos = Vector3.Lerp(startPos, endPos, t);
            _characterController.enabled = false;
            transform.position = nextPos;
            _characterController.enabled = true;

            //transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.enabled = false;
        transform.position = endPos;
        //transform.rotation = endRot;
        _characterController.enabled = true;

        _playerController.SetMovementEnabled(true);
        _isTraversing = false;
    }
}
