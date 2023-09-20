using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static EventManager;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI textElement; // Reference to your TextMeshProUGUI element
    public float blinkSpeed = 0.5f;     // Speed at which the text will blink
    public float startDelay = 1.0f;     // Delay before the blinking starts
    public bool shouldBlink = true;     // Whether the text should blink or not

    [SerializeField] private string windLeftText;
    [SerializeField] private string windRightText;

    private void Start()
    {
        if (textElement == null)
        {
            textElement = GetComponent<TextMeshProUGUI>(); // Get the component if not set
        }
        textElement.text = "";

        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        UpdateEventText();
    }

    private IEnumerator BlinkText()
    {
        yield return new WaitForSeconds(startDelay);

        while (shouldBlink)
        {
            // Lerp alpha from 0 to 1 and then from 1 to 0
            float lerpTime = Mathf.PingPong(Time.time * blinkSpeed, 1);
            Color lerpedColor = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), lerpTime);

            textElement.color = lerpedColor;
            yield return null;
        }
    }

    private void UpdateEventText()
    {
        switch (EventManager.Instance.currentEvent)
        {
            case EventManager.HazardEvent.None:
                textElement.text = "";
                break;
            case EventManager.HazardEvent.WindLeft:
                textElement.text = windLeftText;
                break;
            case EventManager.HazardEvent.WindRight:
                textElement.text = windRightText;
                break;
        }
    }
}

