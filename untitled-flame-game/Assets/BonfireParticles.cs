using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem myParticleSystem;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        velocityOverLifetime = myParticleSystem.velocityOverLifetime;

        velocityOverLifetime.x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (EventManager.Instance.currentEvent)
        {
            case EventManager.HazardEvent.None:
                velocityOverLifetime.x = 0;
                break;
            case EventManager.HazardEvent.WindLeft:
                velocityOverLifetime.x = -5;
                break;
            case EventManager.HazardEvent.WindRight:
                velocityOverLifetime.x = 5;
                break;
        }
    }
}
