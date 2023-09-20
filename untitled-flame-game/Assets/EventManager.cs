// Inside EventManager.cs
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    [SerializeField] private GameObject player;
    private PlayerMovement pm;

    public enum HazardEvent
    {
        None,
        WindLeft,
        WindRight
    }

    public HazardEvent currentEvent;
    public float eventDuration = 10f;
    public float timeUntilNextEvent = 30f; // Time in seconds until the next event

    private float timeOfLastEvent;

    void Awake()
    {
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float elapsedTime = TimeManager.Instance.GetElapsedTime();

        if (elapsedTime - timeOfLastEvent >= timeUntilNextEvent)
        {
            // Trigger a new event
            TriggerRandomEvent();
            timeOfLastEvent = elapsedTime;
        }
    }

    private void TriggerRandomEvent()
    {
        HazardEvent randomEvent = (HazardEvent)Random.Range(1, System.Enum.GetNames(typeof(HazardEvent)).Length);
        StartCoroutine(HandleEvent(randomEvent));
    }

    private IEnumerator HandleEvent(HazardEvent hazardEvent)
    {
        currentEvent = hazardEvent;
        switch (currentEvent)
        {
            case HazardEvent.WindLeft:
                // Apply wind force to the left
                pm.ApplyWindForce(Vector2.left);
                break;

            case HazardEvent.WindRight:
                // Apply wind force to the right
                pm.ApplyWindForce(Vector2.right);
                break;
        }

        yield return new WaitForSeconds(eventDuration);

        // Reset back to no event
        currentEvent = HazardEvent.None;
        pm.isUnderWindEffect = false;
    }
}
