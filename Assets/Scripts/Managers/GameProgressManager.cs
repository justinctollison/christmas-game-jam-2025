using System;
using TMPro;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [SerializeField] public TextMeshProUGUI coinsText;
    [SerializeField] public TextMeshProUGUI lightsText;
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
        Debug.Log("Game Complete!");
        // Jam finish screen / fade / bell sound
    }
}
