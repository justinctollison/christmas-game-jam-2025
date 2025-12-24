using System;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    public int CoinsCollected { get; private set; }
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
    }

    public void RegisterLight()
    {
        TotalLights++;
    }

    public void RegisterLightOn()
    {
        LightsOn++;

        if (LightsOn >= TotalLights)
        {
            OnAllLightsOn?.Invoke();
        }
    }

    public void AddCoin()
    {
        CoinsCollected++;
    }

    public void CompleteGame()
    {
        Debug.Log("Game Complete!");
        // Jam finish screen / fade / bell sound
    }
}
