using UnityEngine;

public interface IInteractable
{
    Transform InteractionPoint { get; }

    void Interact(PlayerInteractor interactor);

    void ShowPrompt();
    void HidePrompt();
}
