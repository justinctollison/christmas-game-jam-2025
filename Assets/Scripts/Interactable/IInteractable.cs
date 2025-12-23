public interface IInteractable
{
    void Interact(PlayerInteractor interactor);
    bool CanInteract { get; }
}
