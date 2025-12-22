using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;

    private void OnEnable()
    {
        CoinPickup.OnCoinCollected += AddScore;
    }

    private void OnDisable()
    {
        CoinPickup.OnCoinCollected -= AddScore;
    }

    private void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }
}