using UnityEngine;
using System.Collections;

public class MerchantManager : MonoBehaviour
{
    public static MerchantManager Instance;

    public GameObject merchantPrefab;
    public GameObject[] shopItemPrefabs; // Array of different Shop Item Prefabs
    public float initialDelay = 45f;
    public float minInterval = 40f;
    public float maxInterval = 60f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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


            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval); // Interval between merchant spawns
        }
    }
}
