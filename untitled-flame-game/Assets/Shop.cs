using UnityEngine;
using TMPro;
using System.Collections;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shopText; // Reference to the blinking TextMeshProUGUI component
    [SerializeField] private TextMeshProUGUI nonBlinkingText; // Reference to the non-blinking TextMeshProUGUI component

    void Start()
    {
        if (shopText != null && nonBlinkingText != null)
        {
            StartCoroutine(BlinkTextCoroutine());
            StartCoroutine(DisplayNonBlinkingTextCoroutine());
        }
        else
        {
            Debug.LogError("One or more Text components are not assigned!");
        }
    }

    private IEnumerator BlinkTextCoroutine()
    {
        // Wait for 1 second before starting the blinking effect
        yield return new WaitForSeconds(1f);

        // Activate the text
        shopText.gameObject.SetActive(true);

        float blinkDuration = 6f; // Total time for blinking
        float blinkSpeed = 1f; // Speed of blinking (change to slow down or speed up)
        float time = 0;

        while (time < blinkDuration)
        {
            // Calculate the alpha value
            float alpha = Mathf.PingPong(time * blinkSpeed, 1);
            shopText.color = new Color(shopText.color.r, shopText.color.g, shopText.color.b, alpha);

            // Increment the time
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // After blinking, deactivate the text
        shopText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayNonBlinkingTextCoroutine()
    {
        nonBlinkingText.gameObject.SetActive(true); // Show the non-blinking text

        yield return new WaitForSeconds(5f); // Wait for 4 seconds

        nonBlinkingText.gameObject.SetActive(false); // Hide the non-blinking text
    }
}
