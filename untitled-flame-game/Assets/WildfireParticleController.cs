using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildfireParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps; 

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.Wildfire)
        {
            PlayParticles();
        }
        if (EventManager.Instance.currentEvent != EventManager.HazardEvent.Wildfire)
        {
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
