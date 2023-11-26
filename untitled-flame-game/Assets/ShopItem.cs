using UnityEngine;
using TMPro;
using System.Collections;

public abstract class ShopItem : MonoBehaviour
{
    protected int logCost; // The cost of the item in logs
    protected abstract void OnPurchase(); // What happens when the item is purchased
    [SerializeField] protected TextMeshProUGUI tmp;

    [Header("Item Text")]
    [SerializeField] private string acquiredText;
    [SerializeField] private string itemText;
    [SerializeField] private string subText;

    [Header("SFX")]
    [SerializeField] static int clipStartIdx = 7;
    [SerializeField] private int clipIdx;
    [SerializeField] static float volume = 0.5f;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && ItemManager.Instance.GetLogCount() >= logCost) // Check if player has enough logs
            {
                ItemManager.Instance.TryDecreaseLogCount(logCost); // Deduct the log cost
                ItemTextController.Instance.ShowItemText(acquiredText, itemText, subText);
                clipIdx = UnityEngine.Random.Range(0, 2) + clipStartIdx;
                SFXManager.Instance.PlaySFX(clipIdx, volume);
                OnPurchase();
                DestroyThisItem();
            }
        }
    }

    private void DestroyThisItem()
    {
        Destroy(gameObject);
    }

    public void SetLogCost(int cost)
    {
        logCost = cost; // Method to set the log cost if needed
    }
}


