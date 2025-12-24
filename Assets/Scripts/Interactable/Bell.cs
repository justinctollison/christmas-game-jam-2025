using UnityEngine;

public class Bell : InteractableBase
{
    public override void Interact(PlayerInteractor interactor)
    {
        GameProgressManager.Instance.CompleteGame();

        HidePrompt();
    }
}
