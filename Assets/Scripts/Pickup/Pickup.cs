using UnityEngine;
using System;

public class CoinPickup : MonoBehaviour
{
    // Event fired when a coin is collected
    public static event Action<int> OnCoinCollected;

    [SerializeField]
    private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Notify listeners
        OnCoinCollected?.Invoke(coinValue);

        // Destroy the coin
        Destroy(gameObject);
    }
}