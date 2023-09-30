using UnityEngine;

public class WestWindParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps; 

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.WindRight)
        {
            PlayParticles();
        }
        if (EventManager.Instance.currentEvent != EventManager.HazardEvent.WindRight) {
            StopParticles();
        }

    }

    void PlayParticles()
    {
        ps.Pause();
        ps.Play();

    }

    void StopParticles()
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
     
    }
}
