using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _interactionPoint;

    public Transform InteractionPoint => _interactionPoint;

    [SerializeField] private Canvas _canvas;

    private Transform _cameraTransform;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();

        if (_canvas != null)
            _canvas.gameObject.SetActive(false);

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

    public void ShowPrompt()
    {
        if (_canvas != null)
            _canvas.gameObject.SetActive(true);
        Debug.Log($"Showing Canvas for {gameObject.name}");
    }

    public void HidePrompt()
    {
        if (_canvas != null)
            _canvas.gameObject.SetActive(false);

    }

    public abstract void Interact(PlayerInteractor interactor);
}
