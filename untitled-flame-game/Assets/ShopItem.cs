using UnityEngine;
using TMPro;
using System.Collections;

public abstract class ShopItem : MonoBehaviour
{
    protected int logCost; // The cost of the item in logs
    protected abstract void OnPurchase(); // What happens when the item is purchased
    [SerializeField] protected TextMeshProUGUI tmp;
    [SerializeField] public bool isStoryItem = false;

    [Header("Item Text")]
    [SerializeField] private string acquiredText;
    [SerializeField] private string itemText;
    [SerializeField] private string subText;
    [SerializeField] protected TextMeshProUGUI[] textComponents;

    [Header("SFX")]
    [SerializeField] static int clipStartIdx = 7;
    [SerializeField] private int clipIdx;
    [SerializeField] private int redClipIdx = 11;
    [SerializeField] static float volume = 0.5f;

    void Start()
    {
        // Set the color of the text based on whether it's a story item or not
        Color textColor = isStoryItem ? new Color(197f / 255f, 51f / 255f, 33f / 255f) : new Color(108f / 255f, 147f / 255f, 48f / 255f);
        foreach (var tmpText in textComponents)
        {
            if (tmpText != null)
            {
                tmpText.color = textColor;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && ItemManager.Instance.GetLogCount() >= logCost) // Check if player has enough logs
            {
                ItemManager.Instance.TryDecreaseLogCount(logCost); // Deduct the log cost
                ItemTextController.Instance.ShowItemText(acquiredText, itemText, subText, isStoryItem);
                if (!isStoryItem)
                {
                    clipIdx = UnityEngine.Random.Range(0, 2) + clipStartIdx;
                    SFXManager.Instance.PlaySFX(clipIdx, volume);
                } else
                {
                    SFXManager.Instance.PlaySFX(redClipIdx, volume);
                }
                
                OnPurchase();
                DestroyThisItem();
            }
        }
    }

    protected virtual void DestroyThisItem()
    {
        Destroy(gameObject);
    }

    public void SetLogCost(int cost)
    {
        logCost = cost; // Method to set the log cost if needed
    }
}


