using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SchoolBackpack : ShopItem
{
    [SerializeField] private int addAmount = 1; // 50% light increase
    [SerializeField] private int costInLogs = 3;

    private void Start()
    {
        logCost = costInLogs;
        tmp.text = logCost.ToString();
    }

    protected override void OnPurchase()
    {
        ItemManager.Instance.IncrementMaxLogs(addAmount);
    }
}