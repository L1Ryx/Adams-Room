using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : ShopItem
{
    [SerializeField] private float lightMultiplier = 1.5f; // 50% light increase
    [SerializeField] private int costInLogs = 2;

    private void Start()
    {
        logCost = costInLogs;
        tmp.text = logCost.ToString();
    }

    protected override void OnPurchase()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Light2D light = player.GetComponentInChildren<Light2D>();

        light.intensity *= lightMultiplier;
    }
}