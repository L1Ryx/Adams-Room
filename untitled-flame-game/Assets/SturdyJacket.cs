using UnityEngine;

public class SturdyJacket : ShopItem
{
    [SerializeField] private float windMultiplier = 0.70f; // 30% speed decrease
    [SerializeField] private int costInLogs = 2;

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
            player.ChangeWindForce(windMultiplier);
        }
    }
}