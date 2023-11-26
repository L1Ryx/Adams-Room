using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance; // Singleton instance

    public int maxLogs = 5; // Maximum number of logs that can be obtained

    private int logCount = 0; // Counter for logs collected

    [SerializeField] protected GameObject bonfireObj;
    protected BonfireManager bm;
    [SerializeField] private float bonfireAddAmount = 0.5f;

    void Awake()
    {
        bonfireObj = GameObject.FindWithTag("TheBonfireObj");
        bm = bonfireObj.GetComponent<BonfireManager>();
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Optionally, make this object persist across scenes
        // DontDestroyOnLoad(gameObject);
    }

    public void IncrementMaxLogs(int amount)
    {
        maxLogs += amount;
    }

    // Attempt to increment the log count; return false if max is reached
    public bool TryIncrementLogCount()
    {
        if (logCount < maxLogs)
        {
            logCount++;
            return true; // Increment was successful
        }
        else { 
            return false; // Increment failed; max count reached
        }
    }

    public bool TryDecreaseLogCount(int amount)
    {
        if (logCount >= amount)
        {
            logCount -= amount; // Decrease was successful
            return true;
        }
        else
        {
            return false; // Decrease failed; not enough logs
        }
    }

    public float GetBonfireAddAmount()
    {
        return bonfireAddAmount;
    }

    // Get the current log count
    public int GetLogCount()
    {
        return logCount;
    }

    // Reset the log count (if needed)
    public void ResetLogCount()
    {
        logCount = 0;
    }

    public void AddToBonfire(int logCount)
    {
        bm.AddBonfireValue(logCount * bonfireAddAmount);
    }
}
