using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // Singleton instance

    [SerializeField] private float elapsedTime = 0.0f; // Time elapsed since the start of the level
    [SerializeField] private bool isPaused = false; // Is the timer paused?

    void Awake()
    {
        isPaused = false;
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            elapsedTime += Time.deltaTime;
            Time.timeScale = 1;
        }
        if (isPaused)
        {
            Time.timeScale = 0;
        }
    }

    // Get the elapsed time
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Reset the elapsed time (if needed)
    public void ResetElapsedTime()
    {
        elapsedTime = 0.0f;
    }

    // Pause the timer
    public void PauseTimer()
    {
        isPaused = true;
    }

    // Resume the timer
    public void ResumeTimer()
    {
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
