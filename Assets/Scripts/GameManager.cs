using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for global access
    public int score = 0;
    public int highScore = 0;
    public Text scoreText; // Assign this in the Inspector
    public Text highScoreText; // Assign this in the Inspector

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadHighScore(); // Load saved high score
        UpdateScoreUI(); // Ensure UI updates at game start
    }

    public void AddScore(int points)
    {
        score += points;

        // Check for new high score immediately
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

        UpdateScoreUI(); // Ensure UI updates right after checking high score
    }


    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}";
        }

        Debug.Log($"Score Updated - Score: {score}, High Score: {highScore}");
    }


    void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
