using UnityEngine;

public class TraversalTarget : MonoBehaviour
{
    [SerializeField] private Transform _landingPoint;
    [SerializeField] private TraversalPrompt _prompt;

    private TraversalDetector _detector;

    public Vector3 LandingPosition => transform.position;
    public Quaternion LandingRotation => transform.rotation;

    private void Awake()
    {
        _detector = FindFirstObjectByType<TraversalDetector>();
        _prompt = GetComponent<TraversalPrompt>();
    }

    private void Update()
    {
        if (_detector == null || _prompt == null)
            return;

        if (_detector.CurrentTarget == this)
            _prompt.Show();
        else
            _prompt.Hide();
    }
}
