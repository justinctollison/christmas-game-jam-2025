using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _interactRange = 1.5f;
    [SerializeField] private LayerMask _interactableMask;

    public IInteractable CurrentTarget { get; private set; }

    private void Update()
    {
        DetectInteractable();
        HandleInteractInput();
    }

    private void DetectInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            _interactRange,
            _interactableMask
        );

        IInteractable newTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                interactable.InteractionPoint.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                newTarget = interactable;
            }
        }

        if (newTarget == CurrentTarget)
            return;

        if (CurrentTarget != null)
            CurrentTarget.HidePrompt();

        CurrentTarget = newTarget;

        if (CurrentTarget != null)
            CurrentTarget.ShowPrompt();
    }

    private void HandleInteractInput()
    {
        if (CurrentTarget == null)
            return;

        if (Input.GetButtonDown("Interact"))
        {
            CurrentTarget.Interact(this);
            GetComponent<Animator>().SetTrigger("Interact");
            Debug.Log("E key was pressed (Interact action)!");
        }

        Debug.Log($"{CurrentTarget} is our current target.");
    }
}
