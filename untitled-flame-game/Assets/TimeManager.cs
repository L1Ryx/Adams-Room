using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // Singleton instance

    [SerializeField] private float elapsedTime = 0.0f; // Time elapsed since the start of the level
    [SerializeField] private bool isPaused = false; // Is the timer paused?

    [Header("Gameplay")]
    public bool shouldShowResults;
    public bool transitioningToResults = false;
    [SerializeField] private string sceneToLoad = "StartScene";

    [Header("Cutscene")]
    public bool shouldShowCutscene = false;
    private bool transitioningToCutscene = false;
    [SerializeField] private string cutsceneToLoad = "movieCutscene";

    [Header("Score")]
    public int score;
    public int logs;

    [Header("Bonfire")]
    public int logCount; // Number of logs collected

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

    public void AddLog(int logsToAdd)
    {
        logCount += logsToAdd;
    }

    // Method to get the current number of logs
    public int GetLogCount()
    {
        return logs;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //ResetElapsedTime();
        //ResetLogCount();
    }

    public void ResetLogCount()
    {
        logCount = 0;
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

        ShopDebug();
    }

    private void ShopDebug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PlayerPrefs.SetInt("shopUnlocked", 0);
            PlayerPrefs.Save();
            Debug.Log("Shop locked (shopUnlocked set to 0)");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerPrefs.SetInt("shopUnlocked", 1);
            PlayerPrefs.Save();
            Debug.Log("Shop unlocked (shopUnlocked set to 1)");
        }
    }


    public void TriggerCutsceneWithDelay(float delay)
    {
        StartCoroutine(TriggerCutsceneAfterDelay(delay));
    }

    private IEnumerator TriggerCutsceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("shouldShowCutscene is true");
        shouldShowCutscene = true;
    }

    private void HandleDeath()
    {
        if (shouldShowResults && !transitioningToResults)
        {
            transitioningToResults = true;
            score = Mathf.FloorToInt(elapsedTime);
            logs = logCount;
            HighScoreManager.Instance.SetHighScore(score);
            FadeManager.Instance.LoadSceneWithFade(sceneToLoad, false);
        }
        if (shouldShowCutscene && !transitioningToCutscene)
        {
            //isPaused = true;
            Debug.Log("transitioning to cutscene");
            score = Mathf.FloorToInt(elapsedTime);
            logs = logCount;
            transitioningToCutscene = true;
            FadeManager.Instance.LoadSceneWithFade(cutsceneToLoad, false);
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
