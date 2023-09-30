using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EastWindParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps; 

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (EventManager.Instance.currentEvent == EventManager.HazardEvent.WindLeft)
        {
            PlayParticles();
        }
        if (EventManager.Instance.currentEvent != EventManager.HazardEvent.WindLeft)
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
