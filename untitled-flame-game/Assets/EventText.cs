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

    [Header("Hazard Messages")]
    [SerializeField] private string windLeftText;
    [SerializeField] private string windRightText;
    [SerializeField] private string darknessText;
    [SerializeField] private string wildfireText;

    [Header("Color Values")]
    [SerializeField] private float colorRed;
    [SerializeField] private float colorGreen;
    [SerializeField] private float colorBlue;

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
            Color lerpedColor = Color.Lerp(new Color(colorRed / 255, colorGreen / 255, colorBlue / 255, 0),
                new Color(colorRed / 255, colorGreen / 255, colorBlue / 255, 1), lerpTime);

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
            case EventManager.HazardEvent.Darkness:
                textElement.text = darknessText;
                break;
            case EventManager.HazardEvent.Wildfire:
                textElement.text = wildfireText;
                break;
        }
    }
}

