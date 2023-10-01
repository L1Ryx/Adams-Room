using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // Singleton instance

    [SerializeField] private float elapsedTime = 0.0f; // Time elapsed since the start of the level
    [SerializeField] private bool isPaused = false; // Is the timer paused?

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> parent of e7d086423 (attempt)
    [Header("Gameplay")]
    public bool shouldShowResults;
    public bool transitioningToResults = false;
    [SerializeField] private string sceneToLoad = "StartScene";

    [Header("Score")]
    public int score;

<<<<<<< HEAD
=======
>>>>>>> parent of 13b62ff7a (first build)
=======
>>>>>>> parent of 13b62ff7a (first build)
=======
>>>>>>> parent of e7d086423 (attempt)
    void Awake()
    {
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            shouldShowResults = Instance.shouldShowResults;
            transitioningToResults = Instance.transitioningToResults;
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        // Optionally, make this object persist across scenes
        // DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetElapsedTime();
    }

    private void Start()
    {
        shouldShowResults = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && !shouldShowResults)
        {
            elapsedTime += Time.deltaTime;
        }
        HandleDeath();

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void HandleDeath()
    {
        if (shouldShowResults && !transitioningToResults)
        {
            transitioningToResults = true;
            score = Mathf.FloorToInt(elapsedTime);
            HighScoreManager.Instance.SetHighScore(score);
            FadeManager.Instance.LoadSceneWithFade(sceneToLoad, false);
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
