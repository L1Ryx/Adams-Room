using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

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

    [Header("Ambiance")]
    [SerializeField] private int clipIdx = 3;
    [SerializeField] private float titleAmbianceVolume = 0.7f;

    void Start()
    {
        isStarting = false;
        AmbianceManager.Instance.PlayAmbiance(clipIdx, titleAmbianceVolume);
        tmp = startText.GetComponent<TextMeshProUGUI>();
        startText.gameObject.SetActive(false); // Initially set to false.
        StartCoroutine(EnableStart());
    }

    private IEnumerator EnableStart()
    {
        yield return new WaitForSeconds(secondsToWait); // Wait for 3 seconds
        canStart = true; // Enable start
        startText.SetActive(true); // Display blinking text
        StartCoroutine(FadeText());
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
        if (canStart && Input.anyKey && !isStarting) // If user is allowed to start and any key is pressed
        {
            isStarting = true;
            AmbianceManager.Instance.FadeOutAndDestroyAll();
            FadeManager.Instance.LoadSceneWithFade(sceneLoadName, false);
            isStarting = false;
        }
    }
}
