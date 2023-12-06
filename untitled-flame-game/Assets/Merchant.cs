using UnityEngine;
using System.Collections;
using TMPro;

public class Merchant : MonoBehaviour
{
    [SerializeField] private Vector2 entryPosition;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 exitPosition;
    [SerializeField] private float speed = 2.0f;

    [SerializeField] private Animator anim;

    [SerializeField] private Vector2 itemOffset = new Vector2(1, 0);
    [SerializeField] private float itemDisappearTime = 8.0f;

    [SerializeField] private GameObject randomItemPrefab;

    [SerializeField] private bool shouldReturn = false;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] storyItemDialogues;
    [SerializeField] private string[] nonStoryItemDialogues;
    [SerializeField] private float letterTypingSpeed = 0.05f;

    private GameObject item;
    private Coroutine displayDialogueCoroutine;

    private GameObject shopItemPrefab; // Variable to hold the assigned shop item prefab

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.transform.position = startPosition;
        if (shopItemPrefab == null)
        {
            Debug.LogError("Shop Item Prefab has not been set!");
            return;
        }
        dialoguePanel.SetActive(false);
        StartCoroutine(MerchantRoutine());
    }

    public void MoveToStop()
    {
        anim.SetTrigger("stop");
    }

    public void MoveToIdle()
    {
        anim.SetTrigger("shouldIdle");
    }

    public void MoveToGo()
    {
        anim.SetTrigger("go");
    }

    public void MoveToTurnLeft()
    {
        anim.SetTrigger("turnLeft");
        shouldReturn = true;
    }

    private IEnumerator MerchantRoutine()
    {
        yield return MoveToPosition(entryPosition);

        MoveToStop();

        // Instantiate the assigned shop item prefab
        item = Instantiate(shopItemPrefab, (Vector2)transform.position + itemOffset, Quaternion.identity);
        ShopItem shopItem = item.GetComponent<ShopItem>();

        dialoguePanel.SetActive(true);

        displayDialogueCoroutine = StartCoroutine(DisplayDialogue(shopItem.isStoryItem));

        yield return new WaitForSeconds(itemDisappearTime); // Wait until the item disappears

        // Now deactivate the dialogue panel
        dialoguePanel.SetActive(false);

        if (item != null)
        {
            Destroy(item);
        }

        yield return new WaitForSeconds(2.0f); // Additional wait time before the merchant leaves

        MoveToGo();

        yield return new WaitUntil(() => shouldReturn);

        yield return MoveToPosition(exitPosition);
        Destroy(gameObject);
    }

    public void SetShopItemPrefab(GameObject prefab)
    {
        shopItemPrefab = prefab;
        // Additional logic to handle the assigned item, if needed
    }

    private IEnumerator DisplayDialogue(bool isStoryItem)
    {
        // Choose the color based on the item type, maintaining the current alpha value
        Color textColor = isStoryItem ? new Color(197f / 255f, 51f / 255f, 33f / 255f, dialogueText.color.a) : new Color(217f / 255f, 206f / 255f, 166f / 255f, dialogueText.color.a);
        dialogueText.color = textColor;

        string dialogue = GetRandomDialogue(isStoryItem);
        dialogueText.text = "";

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterTypingSpeed);
        }

        yield return new WaitForSeconds(itemDisappearTime);
        dialogueText.text = ""; // Clear the text when the item disappears
    }


    private string GetRandomDialogue(bool isStoryItem)
    {
        string[] dialogues = isStoryItem ? storyItemDialogues : nonStoryItemDialogues;
        int randomIndex = Random.Range(0, dialogues.Length);
        return dialogues[randomIndex];
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
