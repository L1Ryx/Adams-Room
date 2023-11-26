using UnityEngine;

public class SquirrelWhistle : ShopItem
{
    [SerializeField] private float logSpawnSlowDownMultiplier = 0.15f;
    [SerializeField] private int costInLogs = 3;

    private void Start()
    {
        logCost = costInLogs;
        tmp.text = logCost.ToString();
    }

    protected override void OnPurchase()
    {
        if (ItemSpawner.Instance != null)
        {
            ItemSpawner.Instance.AdjustSpawnIntervalWithMultiplier(logSpawnSlowDownMultiplier);
        }
    }
}