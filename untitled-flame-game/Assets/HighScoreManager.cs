using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance; // Singleton instance

    private int highScore = 0;
    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
            return; // Important to stop execution of the current instance's Awake method
        }

        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha0)) // Check if '0' key is pressed
        {
            ResetHighScore();
        }
#endif
    }

    public void SetHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save(); // Save changes to disk
        }
    }

#if UNITY_EDITOR
    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
        Debug.Log("High Score Reset to 0");
    }
#endif

    public int GetHighScore()
    {
        return highScore;
    }
}
