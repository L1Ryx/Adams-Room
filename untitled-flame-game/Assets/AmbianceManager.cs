using UnityEngine;
using System.Collections;

public class AmbianceManager : MonoBehaviour
{
    public static AmbianceManager Instance;

    [SerializeField] private AudioClip[] ambianceClips;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private float leadTime = 2.0f;

    private AudioSource currentAudioSource;
    private AudioSource nextAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var clip in ambianceClips)
        {
            clip.LoadAudioData();
        }
    }

    public void PlayAmbiance(int clipIndex, float volume)
    {
        if (clipIndex < 0 || clipIndex >= ambianceClips.Length)
        {
            Debug.LogError("Invalid AudioClip index");
            return;
        }

        if (currentAudioSource == null)
        {
            currentAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
            currentAudioSource.clip = ambianceClips[clipIndex];
            currentAudioSource.loop = true;
            currentAudioSource.volume = 0f; // Ensure volume is 0 before playing
            currentAudioSource.Play();
            StartCoroutine(FadeIn(currentAudioSource, volume));
        }
        else
        {
            StartCoroutine(SwitchAmbiance(clipIndex, volume));
        }
    }


    private IEnumerator FadeOut(AudioSource source, float targetVolume)
    {
        
        float startTime = Time.time;
        float startVolume = source.volume;

        while (Time.time - startTime < fadeTime)
        {
            float elapsed = Time.time - startTime;
            if (source == null) yield break; // Add this check
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeTime);
            yield return null;
        }

        source.volume = targetVolume;
    }

    private IEnumerator SwitchAmbiance(int clipIndex, float volume)
    {
        AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        newAudioSource.clip = ambianceClips[clipIndex];
        newAudioSource.loop = true;
        newAudioSource.Play();
        newAudioSource.volume = 0f;

        // Start fading in the new clip and fading out the old clip simultaneously
        StartCoroutine(FadeIn(newAudioSource, volume));
        StartCoroutine(FadeOut(currentAudioSource, 0f));

        // Wait for the old clip to finish fading out before destroying it
        yield return new WaitForSeconds(fadeTime);

        if (currentAudioSource != null)
        {
            StartCoroutine(FadeOut(currentAudioSource, 0f));
            yield return new WaitForSeconds(fadeTime);
            Destroy(currentAudioSource.gameObject);
        }

        // Assign the new audio source as the current audio source
        currentAudioSource = newAudioSource;
    }

    private IEnumerator ManageLoopingAmbiance(AudioSource source, float volume)
    {
        while (source.isPlaying)
        {
            if (source.clip.length - source.time <= leadTime + fadeTime && nextAudioSource == null)
            {
                nextAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
                nextAudioSource.clip = source.clip;
                nextAudioSource.PlayScheduled(AudioSettings.dspTime + source.clip.length - source.time - (leadTime + fadeTime / 2.0f));
                nextAudioSource.volume = 0f;
                StartCoroutine(Crossfade(source, nextAudioSource, volume));
            }
            yield return null;
        }

        Destroy(source.gameObject);
        if (source == currentAudioSource)
        {
            currentAudioSource = nextAudioSource;
            nextAudioSource = null;
            if (currentAudioSource != null)
            {
                currentAudioSource.loop = true; // Set the next audio source to loop
                StartCoroutine(ManageLoopingAmbiance(currentAudioSource, volume));
            }
        }
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume)
    {
        float startTime = Time.time;
        while (Time.time - startTime < fadeTime)
        {
            float t = (Time.time - startTime) / fadeTime;
            source.volume = t * targetVolume;
            yield return null;
        }
        source.volume = targetVolume;
    }


    private IEnumerator Crossfade(AudioSource oldSource, AudioSource newSource, float targetVolume)
    {
        float startTime = Time.time;
        while (Time.time - startTime < fadeTime)
        {
            float t = (Time.time - startTime) / fadeTime;
            oldSource.volume = (1 - t) * targetVolume;
            newSource.volume = t * targetVolume;
            yield return null;
        }
    }

    public void FadeOutAndDestroyAll()
    {
        if (currentAudioSource != null)
        {
            StopCoroutine(ManageLoopingAmbiance(currentAudioSource, currentAudioSource.volume));
            StartCoroutine(FadeOutAndDestroyCoroutine(currentAudioSource));
        }

        if (nextAudioSource != null)
        {
            StopCoroutine(Crossfade(currentAudioSource, nextAudioSource, nextAudioSource.volume));
            StartCoroutine(FadeOutAndDestroyCoroutine(nextAudioSource));
        }
    }


    private IEnumerator FadeOutAndDestroyCoroutine(AudioSource source)
    {
        
        float startTime = Time.time;
        float startVolume = source.volume;

        while (Time.time - startTime < fadeTime)
        {
            float elapsed = Time.time - startTime;
            if (source == null) yield break;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            yield return null;
        }
        if (source == null) yield break;
        Destroy(source.gameObject);
        if (source == currentAudioSource) currentAudioSource = null;
        if (source == nextAudioSource) nextAudioSource = null;
    }
}
