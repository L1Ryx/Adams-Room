using UnityEngine;

public class OldSneakers : ShopItem
{
    [SerializeField] private float speedMultiplier = 1.20f; // 20% speed increase
    [SerializeField] private int costInLogs = 4;

    private void Start()
    {
        logCost = costInLogs;
        tmp.text = logCost.ToString();
    }

    protected override void OnPurchase()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.IncreaseSpeed(speedMultiplier); // Call a method to increase the player’s speed
        }
    }
}