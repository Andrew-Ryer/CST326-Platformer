using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    LevelParser _level;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;

    private int score = 0;
    private int coins = 0;
    private float timeLeft = 100f;
    
    void Awake()
    {
        _level = FindFirstObjectByType<LevelParser>();
    }

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
        //PT2
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeLeft = 0;
            timeText.text = $"TIME\n{timeLeft.ToString("#")}";
            Debug.Log("YOU FAILED!");
            OnPlayerDied();
            return;
        }

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
    
    //PT2
    public void ReloadLevel()
    { 
        _level.ReloadLevel();
    }
    
    //PT2
    public void OnPlayerDied()
    {
        ReloadLevel();
    }

    //PT2
    public void OnPlayerReachedGoal()
    {
        Debug.Log("YOU WIN!");
        ReloadLevel();
    }
}