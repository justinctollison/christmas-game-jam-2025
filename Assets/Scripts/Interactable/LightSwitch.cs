using UnityEngine;

public class LightSwitch : InteractableBase
{
    [SerializeField] private Light _light;
    [SerializeField] private SkinnedMeshRenderer _lightShaft;

    private bool _isOn;

    private void Start()
    {
        _light.enabled = false;
        _lightShaft.enabled = false;
        GameProgressManager.Instance.RegisterLight();
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (_isOn)
            return;

        _isOn = true;
        _light.enabled = true;
        _lightShaft.enabled = true;

        GameProgressManager.Instance.RegisterLightOn();
        AudioManager.Instance.PlaySFX(SFXType.LightSwitch);
        HidePrompt();
    }
}
