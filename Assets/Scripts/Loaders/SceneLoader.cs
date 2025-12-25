using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //AudioManager.Instance.PlayBGM(BGMType.MainMenu);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);

        AudioManager.Instance.PlayBGM(BGMType.MainMenu);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);

        AudioManager.Instance.PlayBGM(BGMType.Background);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
