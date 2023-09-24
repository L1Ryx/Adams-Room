using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Log : Item
{
    [Header("Lighting Settings")]
    [SerializeField] private GameObject lightObj;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private float originalLightIntensity;
    [SerializeField] private float darknessTransitionTime = 2.5f;
    private Coroutine changeIntensityCoroutine;
    private bool isTransitioning;
    private void Awake()
    {
        lightComponent = lightObj.GetComponent<Light2D>();
        originalLightIntensity = lightComponent.intensity;
        SetInitialLightIntensityBasedOnEvent();
    }

    private void SetInitialLightIntensityBasedOnEvent()
    {
        float targetIntensity = EventManager.Instance.currentEvent == EventManager.HazardEvent.Darkness ? 0f : originalLightIntensity;
        if (lightComponent.intensity != targetIntensity)
            StartChangeIntensityCoroutine(targetIntensity, darknessTransitionTime);
    }

    private void Update()
    {
        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.Darkness && !Mathf.Approximately(lightComponent.intensity, 0f) && !isTransitioning)
        {
            StartChangeIntensityCoroutine(0f, darknessTransitionTime);
        }
        else if (EventManager.Instance.currentEvent == EventManager.HazardEvent.None && !Mathf.Approximately(lightComponent.intensity, originalLightIntensity) && !isTransitioning)
        {
            StartChangeIntensityCoroutine(originalLightIntensity, darknessTransitionTime);
        }
    }

    private void StartChangeIntensityCoroutine(float targetIntensity, float duration)
    {
        if (changeIntensityCoroutine != null)
            StopCoroutine(changeIntensityCoroutine);

        changeIntensityCoroutine = StartCoroutine(ChangeLightIntensityOverTime(targetIntensity, duration));
    }

    private IEnumerator ChangeLightIntensityOverTime(float targetIntensity, float duration)
    {
        isTransitioning = true;
        float startTime = Time.time;
        float initialIntensity = lightComponent.intensity;

        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            lightComponent.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsed / duration);
            yield return null;
        }

        lightComponent.intensity = targetIntensity;
        isTransitioning = false;
    }
    public override void OnPickup()
    {
        if (ItemManager.Instance.TryIncrementLogCount())
        {
            //bm.bonfireValue += 5f;
            DestroyAndMarkPositionFree();
        }
    }

    public override bool CanPickUp()
    {
        return ItemManager.Instance.GetLogCount() < ItemManager.Instance.maxLogs;
    }

    // Added this new method
    private void DestroyAndMarkPositionFree()
    {
        ItemSpawner.Instance.MarkPositionAsFree((int)gridPosition.x, (int)gridPosition.y);
        Destroy(gameObject);
    }

}
