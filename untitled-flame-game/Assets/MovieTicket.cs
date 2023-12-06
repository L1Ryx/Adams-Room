using UnityEngine;

public class MovieTicket : ShopItem
{
    [SerializeField] private int costInLogs = 6;
    private float cutsceneDelay = 5f; // 5 seconds delay

    private void Awake()
    {
        isStoryItem = true;
    }

    private void Start()
    {
        logCost = costInLogs;
        tmp.text = logCost.ToString();
    }

    protected override void DestroyThisItem()
    {
        base.DestroyThisItem();
    }

    protected override void OnPurchase()
    {
        PlayerPrefs.SetInt("shopUnlocked", 1);
        TimeManager.Instance.TriggerCutsceneWithDelay(cutsceneDelay);
    }
}
