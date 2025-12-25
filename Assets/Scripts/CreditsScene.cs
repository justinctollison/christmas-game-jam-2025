using UnityEngine;

public class CreditsScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMType.Credits);
    }

    public void ReturnToMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
