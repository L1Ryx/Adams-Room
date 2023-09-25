using UnityEngine;
using System.Collections;

public class AmbianceManager : MonoBehaviour
{
    public static AmbianceManager Instance;

    [SerializeField] private AudioClip[] ambianceClips;
    [SerializeField] private GameObject audioSourcePrefab; // Assume you have an AudioSource prefab
    [SerializeField] private float fadeTime = 2.0f;

    private AudioSource currentAudioSource;

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
        currentAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        currentAudioSource.clip = ambianceClips[0];
        currentAudioSource.Play();
    }

    public void PlayAmbiance(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= ambianceClips.Length)
        {
            Debug.LogError("Invalid AudioClip index");
            return;
        }

        if (currentAudioSource.clip == ambianceClips[clipIndex] && currentAudioSource.isPlaying)
            return;

        AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        newAudioSource.clip = ambianceClips[clipIndex];
        newAudioSource.Play();
        StartCoroutine(Crossfade(currentAudioSource, newAudioSource));
    }

    private IEnumerator Crossfade(AudioSource oldSource, AudioSource newSource)
    {
        float startTime = Time.time;
        float startVolume = oldSource.volume;

        newSource.volume = 0f;

        while (Time.time - startTime < fadeTime)
        {
            float elapsed = Time.time - startTime;
            oldSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            newSource.volume = Mathf.Lerp(0f, startVolume, elapsed / fadeTime);
            yield return null;
        }

        oldSource.Stop();
        Destroy(oldSource.gameObject);
        currentAudioSource = newSource;
    }
}
