using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Log : Item
{
    [Header("SFX")]
    [SerializeField] private int woodCollectIdxLow = 4;
    [SerializeField] private int woodCollectIdxHigh = 7;
    [SerializeField] private float woodCollectVolume = 0.4f;

    [Header("Burn Settings")]
    [SerializeField] private float burnDuration = 5f; // Example duration
    [SerializeField] private float dieAnimationLength = 1.125f; // Example duration
    [SerializeField] private bool isDying = false;
    private Coroutine burnCoroutine;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float spawnTransitionLength;
    [SerializeField] private bool isIdle = false;

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
        animator = GetComponent <Animator>();
        SetInitialLightIntensityBasedOnEvent();
    }

    private void Start()
    {
        StartCoroutine(StartIdleAnimationWhenSpawnFinishes());
    }

    private IEnumerator StartIdleAnimationWhenSpawnFinishes()
    {
        // Assuming your spawn animation duration is 1 second. Change this to your spawn animation duration.
        yield return new WaitForSeconds(spawnTransitionLength);
        animator.SetTrigger("toIdle");
        isIdle = true;
    }

    private void SetInitialLightIntensityBasedOnEvent()
    {
        float targetIntensity = EventManager.Instance.currentEvent == EventManager.HazardEvent.Darkness ? 0f : originalLightIntensity;
        if (lightComponent.intensity != targetIntensity)
            StartChangeIntensityCoroutine(targetIntensity, darknessTransitionTime);
    }

    private void Update()
    {
        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.Wildfire && burnCoroutine == null)
        {
            // Start burning coroutine when a wildfire starts
            burnCoroutine = StartCoroutine(BurnCoroutine());
        }
        else if (EventManager.Instance.currentEvent != EventManager.HazardEvent.Wildfire && burnCoroutine != null)
        {
            // Stop burning coroutine when wildfire stops
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
            animator.SetTrigger("toIdle"); // Or any other way to reset to a safe state
        }

        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.Darkness && !Mathf.Approximately(lightComponent.intensity, 0f) && !isTransitioning)
        {
            StartChangeIntensityCoroutine(0f, darknessTransitionTime);
        }
        else if (EventManager.Instance.currentEvent == EventManager.HazardEvent.None && !Mathf.Approximately(lightComponent.intensity, originalLightIntensity) && !isTransitioning)
        {
            StartChangeIntensityCoroutine(originalLightIntensity, darknessTransitionTime);
        }
    }

    private IEnumerator BurnCoroutine()
    {
        animator.SetTrigger("toBurn");
        yield return new WaitForSeconds(burnDuration);

        StartCoroutine(DustDeathCoroutine());
    }

    private IEnumerator DustDeathCoroutine()
    {
        isDying = true;
        animator.SetTrigger("toDie");
        yield return new WaitForSeconds(dieAnimationLength);

        DestroyAndMarkPositionFree();
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
            int woodCollectIdx = UnityEngine.Random.Range(woodCollectIdxLow, woodCollectIdxHigh + 1);
            SFXManager.Instance.PlaySFX(woodCollectIdx, woodCollectVolume);
            DestroyAndMarkPositionFree();
        }
    }

    public override bool CanPickUp()
    {
        return (ItemManager.Instance.GetLogCount() < ItemManager.Instance.maxLogs) && !isDying && isIdle;
    }

    // Added this new method
    private void DestroyAndMarkPositionFree()
    {
        ItemSpawner.Instance.MarkPositionAsFree((int)gridPosition.x, (int)gridPosition.y);
        Destroy(gameObject);
    }

}
