using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMType.MainMenu);
    }

    public void LoadCreditsScene()
    {
        SceneLoader.Instance.LoadCredits();
    }

    public void LoadGameScene()
    {
        SceneLoader.Instance.LoadGame();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
