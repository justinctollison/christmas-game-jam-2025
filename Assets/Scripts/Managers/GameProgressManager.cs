using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    private float _gameStartTime;
    private float _gameEndTime;
    private bool _gameCompleted;


    [SerializeField] public TextMeshProUGUI coinsText;
    [SerializeField] public TextMeshProUGUI lightsText;

    [SerializeField] public TextMeshProUGUI peppermintScoreText;
    [SerializeField] public TextMeshProUGUI timeScoreText;

    [SerializeField] public Transform winPanel;

    public int CoinsCollected { get; private set; }
    public int TotalCoins { get; private set; }
    public int TotalLights { get; private set; }
    public int LightsOn { get; private set; }

    public event Action OnAllLightsOn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CoinsCollected = 0;
        TotalLights = 0;
    }

    private void Start()
    {
        _gameStartTime = Time.time;
    }

    private void Update()
    {
        coinsText.text = $"{CoinsCollected}/{TotalCoins}";
        lightsText.text = $"{LightsOn}/{TotalLights}";
    }

    public void RegisterLight()
    {
        TotalLights++;
    }

    public void RegisterCoin()
    {
        TotalCoins++;
    }

    public void RegisterLightOn()
    {
        LightsOn++;

        lightsText.text = $"{LightsOn}/{TotalLights}";

        if (LightsOn >= TotalLights)
        {
            OnAllLightsOn?.Invoke();
        }
    }

    public void AddCoin()
    {
        CoinsCollected++;

        coinsText.text = $"{CoinsCollected}/{TotalCoins}";
    }

    public void CompleteGame()
    {
        winPanel.gameObject.SetActive(true);

        _gameCompleted = true;
        _gameEndTime = Time.time;

        peppermintScoreText.text = $"{CoinsCollected}/{TotalCoins}";
        timeScoreText.text = GetFormattedTime();

        EndGameScreen();
        Debug.Log("Game Complete!");
        AudioManager.Instance.PlaySFX(SFXType.Bell);
        // Jam finish screen / fade / bell soun
    }

    public float GetElapsedTime()
    {
        return _gameEndTime - _gameStartTime;
    }

    public string GetFormattedTime()
    {
        float elapsed = GetElapsedTime();

        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        return $"{minutes:00}:{seconds:00}";
    }

    private void EndGameScreen()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        AudioManager.Instance.PlayBGM(BGMType.Winning);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadMainMenu();
    }
}
