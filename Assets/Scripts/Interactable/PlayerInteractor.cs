using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] LayerMask interactableMask;

    IInteractable currentTarget;

    void Update()
    {
        FindInteractable();

        if (currentTarget != null && Input.GetButtonDown("Interact"))
        {
            currentTarget.Interact(this);
        }
    }

    void FindInteractable()
    {
        currentTarget = null;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactRange,
            interactableMask
        );

        if (hits.Length == 0) return;

        currentTarget = hits[0].GetComponent<IInteractable>();
    }
}
