using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MerchantManager : MonoBehaviour
{
    public static MerchantManager Instance;

    public GameObject merchantPrefab;
    public GameObject[] shopItemPrefabs; // Array of different Shop Item Prefabs
    public float initialDelay = 45f;
    public float minInterval = 40f;
    public float maxInterval = 60f;

    private Queue<GameObject> itemQueue; // Queue to track and cycle through shop items

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeItemQueue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnMerchant());
    }

    private IEnumerator SpawnMerchant()
    {
        yield return new WaitForSeconds(initialDelay); // Initial delay before first spawn

        while (true)
        {
            GameObject merchant = Instantiate(merchantPrefab);
            AssignRandomShopItem(merchant);

            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval); // Interval between merchant spawns
        }
    }

    private void InitializeItemQueue()
    {
        List<GameObject> shuffledItems = new List<GameObject>(shopItemPrefabs);
        ShuffleList(shuffledItems);
        itemQueue = new Queue<GameObject>(shuffledItems);
    }

    private void AssignRandomShopItem(GameObject merchant)
    {
        if (itemQueue.Count == 0)
        {
            InitializeItemQueue(); // Reinitialize the queue if all items have been used
        }

        var randomItemPrefab = itemQueue.Dequeue();
        merchant.GetComponent<Merchant>().SetShopItemPrefab(randomItemPrefab);
    }

    private void ShuffleList(List<GameObject> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
