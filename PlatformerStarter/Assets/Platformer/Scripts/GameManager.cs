using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;

    private int score = 0;
    private int coins = 0;
    private float timeLeft = 360f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize UI immediately so it shows values at start
        scoreText.text = $"Mario\n{score.ToString("D5")}";
        coinsText.text = $"Ox{coins.ToString("D2")}";
        timeText.text = $"TIME\n{timeLeft.ToString("#")}";
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;

        timeText.text = $"TIME\n{timeLeft.ToString("#")}";
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Mario\n{score.ToString("D5")}";
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        coinsText.text = $"Ox{coins.ToString("D2")}";
    }
}