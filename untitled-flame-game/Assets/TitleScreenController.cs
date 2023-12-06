using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEditor;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private string sceneLoadName;
    [SerializeField] private float blinkSpeed = 1f; // Adjust the speed of blinking.
    [SerializeField] private float secondsToWait = 5f;

    private bool canStart = false; // To check if user is allowed to start.
    private bool isStarting;
    private float alphaValue = 0; // For fading effect.
    private bool fadeIn = true; // To check if text is fading in or out.

    [SerializeField] private ScoreReportText srt;

    [Header("Ambiance")]
    [SerializeField] private int clipIdx = 3;
    [SerializeField] private float titleAmbianceVolume = 0.95f;

    [Header("Shop")]
    [SerializeField] private GameObject shopObject;

    void Start()
    {
        TimeManager.Instance.ResetElapsedTime();
        TimeManager.Instance.ResetLogCount();
        FadeManager.Instance.FadeIn();
        isStarting = false;
        TimeManager.Instance.ResumeTimer();
        AmbianceManager.Instance.PlayAmbiance(clipIdx, titleAmbianceVolume);
        tmp = startText.GetComponent<TextMeshProUGUI>();
        startText.gameObject.SetActive(false); // Initially set to false.

       
        if (TimeManager.Instance.shouldShowResults)
        {
            Debug.Log("You survived for " + TimeManager.Instance.score);
            Debug.Log("Your best time is " + HighScoreManager.Instance.GetHighScore());
        }

        CheckShopUnlockStatus();
        StartCoroutine(EnableStart());
    }

    private void CheckShopUnlockStatus()
    {
        bool isShopUnlocked = PlayerPrefs.GetInt("shopUnlocked", 0) == 1;
        if (shopObject != null)
        {
            shopObject.SetActive(isShopUnlocked);
        }
        else
        {
            Debug.LogError("Shop object is not assigned in the TitleScreenManager.");
        }
    }

    private IEnumerator EnableStart()
    {
        yield return new WaitForSeconds(secondsToWait); // Wait for 3 seconds
        canStart = true; // Enable start
        startText.SetActive(true); // Display blinking text
        StartCoroutine(FadeText());
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        // If running in the editor, stop play mode
        EditorApplication.isPlaying = false;
#else
        // If running outside the editor, quit the application
        Application.Quit();
#endif
    }

    public void PlayQuitUIClick()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.uiClickIdx, SFXManager.Instance.uiClickVolume);
    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            if (fadeIn)
                alphaValue += Time.deltaTime * blinkSpeed;
            else
                alphaValue -= Time.deltaTime * blinkSpeed;

            if (alphaValue >= 1f)
                fadeIn = false;
            if (alphaValue <= 0f)
                fadeIn = true;

            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alphaValue);

            yield return null;
        }
    }

    void Update()
    {
        if (canStart && IsAnyKey() && !isStarting) // If user is allowed to start and any key is pressed
        {
            isStarting = true;
            AmbianceManager.Instance.FadeOutAndDestroyAll();
            FadeManager.Instance.LoadSceneWithFade(sceneLoadName, false);
            TimeManager.Instance.ResetElapsedTime();
            isStarting = false;
        }
    }

    bool IsAnyKey()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            // Exclude mouse button key codes.
            if ((int)keyCode >= (int)KeyCode.Mouse0 && (int)keyCode <= (int)KeyCode.Mouse6)
                continue;

            if (Input.GetKey(keyCode))
                return true;
        }
        return false;
    }
}
