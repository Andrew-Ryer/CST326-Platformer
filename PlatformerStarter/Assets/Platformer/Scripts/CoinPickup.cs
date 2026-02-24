using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    public int scoreValue = 100;

    GameManager _gm;

    void Awake()
    {
        _gm = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _gm.AddCoin(coinValue);
        _gm.AddScore(scoreValue);
        
        Destroy(gameObject);
    }
}