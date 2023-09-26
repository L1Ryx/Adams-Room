using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements.Experimental;

public class AmbianceManager : MonoBehaviour
{
    public static AmbianceManager Instance;

    [SerializeField] private AudioClip[] ambianceClips;
    [SerializeField] private GameObject audioSourcePrefab; // Assume you have an AudioSource prefab
    [SerializeField] private float fadeTime = 2.0f;

    [SerializeField] private AudioSource currentAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        // Initialize with an AudioSource playing the first clip
        //currentAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        //if (currentAudioSource.clip == null)
        //{
        //    currentAudioSource.clip = ambianceClips[0];
        //}
        //currentAudioSource.Play();
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
        }

        if (currentAudioSource.clip == ambianceClips[clipIndex] && currentAudioSource.isPlaying)
            return;

        AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        newAudioSource.clip = ambianceClips[clipIndex];
        newAudioSource.volume = 0f; // Set to 0 before starting Crossfade.
        newAudioSource.Play();
        StartCoroutine(Crossfade(currentAudioSource, newAudioSource, volume));
    }

    private IEnumerator Crossfade(AudioSource oldSource, AudioSource newSource, float targetVolume)
    {
        float startTime = Time.time;
        float startVolume = oldSource.volume;

        while (Time.time - startTime < fadeTime)
        {
            float elapsed = Time.time - startTime;
            oldSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            newSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / fadeTime); // Lerp to targetVolume instead of startVolume
            yield return null;
        }
        newSource.volume = targetVolume;

        oldSource.Stop();
        Destroy(oldSource.gameObject);
        currentAudioSource = newSource;
    }

    public void FadeOutAndDestroyAll()
    {
        // If there's no current audio source, there's nothing to fade out or destroy
        if (currentAudioSource == null)
            return;

        StartCoroutine(FadeOutAndDestroyCoroutine());
    }

    private IEnumerator FadeOutAndDestroyCoroutine()
    {
        float startTime = Time.time;
        float startVolume = currentAudioSource.volume;

        while (Time.time - startTime < fadeTime)
        {
            float elapsed = Time.time - startTime;
            currentAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            yield return null;
        }

        // Stop and destroy the current audio source
        currentAudioSource.Stop();
        Destroy(currentAudioSource.gameObject);
        currentAudioSource = null;

        // Find and destroy any remaining AudioSource components under AmbianceManager
        foreach (var audioSource in GetComponentsInChildren<AudioSource>())
        {
            Destroy(audioSource.gameObject);
        }
    }



}
