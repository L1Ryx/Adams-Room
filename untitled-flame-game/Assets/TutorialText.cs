using UnityEngine;
using UnityEngine.UI; // Import for Image
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public List<TextMeshProUGUI> tutorialTexts; // Assign your TextMeshPro Texts in the inspector
    public List<Image> tutorialImages;
    public float fadeDuration = 1.0f;
    public float displayDuration = 10.0f;
    public float initialDelay = 3.0f; // Delay before the texts start appearing

    private void Start()
    {
        // Disable raycasting on all TextMeshProUGUI components
        foreach (var text in tutorialTexts)
        {
            text.raycastTarget = false;

            // Initializing alpha to 0 as per previous conversation.
            var color = text.color;
            color.a = 0f;
            text.color = color;
        }

        // Disable raycasting on all Image components
        foreach (var image in tutorialImages)
        {
            image.raycastTarget = false;

            // Initializing alpha to 0 as per previous conversation.
            var color = image.color;
            color.a = 0f;
            image.color = color;
        }

        StartCoroutine(DisplayAndFadeTutorialElements());
    }

    private IEnumerator DisplayAndFadeTutorialElements()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        // Start Fading In
        StartCoroutine(FadeInElements());

        // Display for certain duration after the fade in is complete
        yield return new WaitForSeconds(displayDuration + fadeDuration);

        // Start Fading Out
        StartCoroutine(FadeOutElements());
    }

    private IEnumerator FadeInElements()
    {
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            foreach (var text in tutorialTexts)
            {
                var color = text.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                text.color = color;
            }

            foreach (var image in tutorialImages)
            {
                var color = image.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                image.color = color;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOutElements()
    {
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            foreach (var text in tutorialTexts)
            {
                var color = text.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                text.color = color;
            }

            foreach (var image in tutorialImages)
            {
                var color = image.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                image.color = color;
            }
            yield return null;
        }
    }
}
