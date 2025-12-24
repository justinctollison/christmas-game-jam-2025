using UnityEngine;

public class Coin : InteractableBase
{
    public override void Interact(PlayerInteractor interactor)
    {
        GameProgressManager.Instance.AddCoin();
        HidePrompt();
        Destroy(gameObject);
    }
}
