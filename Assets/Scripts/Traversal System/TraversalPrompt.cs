using UnityEngine;

public class TraversalPrompt : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0.3f, 0f);

    private Transform _cameraTransform;

    private void Awake()
    {
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
        transform.rotation = Quaternion.LookRotation(
            transform.position - _cameraTransform.position
        );
    }

    public void Show()
    {
        if (_canvas != null)
            _canvas.enabled = true;
    }

    public void Hide()
    {
        if (_canvas != null)
            _canvas.enabled = false;
    }
}
