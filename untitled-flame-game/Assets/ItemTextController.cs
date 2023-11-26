using System.Collections;
using UnityEngine;
using TMPro;

public class ItemTextController : MonoBehaviour
{
    public static ItemTextController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI acquiredText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI subText;
    [SerializeField] private float fadeTime = 1.0f; // Fade time for each text
    [SerializeField] private float fadeOutTimeMultiplier = 5.0f;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure text components start with an alpha of 0
        acquiredText.alpha = 0;
        itemText.alpha = 0;
        subText.alpha = 0;
    }

    public void ShowItemText(string acquired, string item, string sub)
    {
        acquiredText.text = acquired;
        itemText.text = item;
        subText.text = sub;

        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        // Fade in acquiredText and then wait
        yield return StartCoroutine(FadeText(acquiredText, true));
        yield return new WaitForSeconds(fadeTime);

        // Fade in itemText and then wait
        yield return StartCoroutine(FadeText(itemText, true));
        yield return new WaitForSeconds(fadeTime);

        // Fade in subText and then wait
        yield return StartCoroutine(FadeText(subText, true));
        yield return new WaitForSeconds(fadeTime * fadeOutTimeMultiplier);

        // Start fade-out for all texts simultaneously
        StartCoroutine(FadeText(acquiredText, false));
        StartCoroutine(FadeText(itemText, false));
        yield return StartCoroutine(FadeText(subText, false));  // Wait for the last one to ensure all are done
    }

    private IEnumerator FadeText(TextMeshProUGUI text, bool fadeIn)
    {
        float startAlpha = fadeIn ? 0 : 1;
        float endAlpha = fadeIn ? 1 : 0;
        float elapsedTime = 0;

        text.alpha = startAlpha;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            yield return null;
        }

        text.alpha = endAlpha;
    }
}
