using UnityEngine;

public class TraversalPrompt : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private Transform _cameraTransform;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();

        if (_canvas != null)
            _canvas.enabled = false;

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_cameraTransform == null)
            return;

        // Billboard toward camera
        _canvas.transform.rotation = Quaternion.LookRotation(
            _canvas.transform.position - _cameraTransform.position
        );
    }

    public void Show()
    {
        if (_canvas != null)
            _canvas.enabled = true;
        Debug.Log($"Showing Canvas for {gameObject.name}");
    }

    public void Hide()
    {
        if (_canvas != null)
            _canvas.enabled = false;
    }
}
