using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private AudioClip[] audioClips;

    [Header("Footsteps")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float footstepVolume = 0.8f;

    [Header("UI")]
    public int uiClickIdx = 7;
    public float uiClickVolume = 0.8f;

    private void Awake()
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

    private void Start()
    {
        foreach (var clip in audioClips)
        {
            clip.LoadAudioData();
        }
        foreach (var clip in footstepSounds)
        {
            clip.LoadAudioData();
        }
    }
    public void PlayFootstepSound(Vector3 position)
    {
        if (footstepSounds.Length == 0) return; // Do nothing if no footstep sounds are assigned

        int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
        AudioClip clip = footstepSounds[randomIndex];

        // Create a new GameObject for each footstep sound played
        GameObject audioObject = new GameObject("FootstepSFX");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // Volume!!!
        audioSource.volume = footstepVolume;

        // Set the AudioClip and play it
        audioSource.clip = clip;
        audioSource.Play();

        // Move the audioObject to the position where the sound should be played
        audioObject.transform.position = position;

        // Destroy the GameObject after the sound effect has played
        Destroy(audioObject, clip.length);
    }

    public void PlaySFX(int clipIndex, float volume)
    {
        if (clipIndex < 0 || clipIndex >= audioClips.Length)
        {
            Debug.LogError("Invalid AudioClip index");
            return;
        }

        // Create a new GameObject for each sound effect played
        GameObject audioObject = new GameObject("SFX");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // Set the AudioClip and play it
        audioSource.clip = audioClips[clipIndex];
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy the GameObject after the sound effect has played
        Destroy(audioObject, audioClips[clipIndex].length);
    }
}
