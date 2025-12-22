using UnityEngine;

public class TraversalTarget : MonoBehaviour
{
    [SerializeField] private Transform _landingPoint;
    [SerializeField] private TraversalPrompt _prompt;

    public Vector3 LandingPosition => _landingPoint.position;
    public Quaternion LandingRotation => _landingPoint.rotation;

    public void ShowPrompt()
    {
        if (_prompt != null)
            _prompt.Show();
    }

    public void HidePrompt()
    {
        if (_prompt != null)
            _prompt.Hide();
    }
}
