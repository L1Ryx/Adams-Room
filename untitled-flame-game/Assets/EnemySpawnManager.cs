using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    public GameObject greenGhostPrefab;
    public GameObject redGhostPrefab;
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 20f;
    public bool fireIsOut = false;

    private bool spawnImmediately = false;


    private Camera mainCamera;

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

    public void SetFireIsOut(bool status)
    {
        if (fireIsOut != status)
        {
            fireIsOut = status;
            spawnImmediately = true; // Trigger immediate spawn when status changes
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnGhosts());
    }

    private IEnumerator SpawnGhosts()
    {
        while (true)
        {
            float spawnTime = fireIsOut ? Random.Range(minSpawnTime, maxSpawnTime) / 10f : Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);

            if (spawnImmediately)
            {
                spawnImmediately = false;
                continue; // Skip the current waiting time and start a new spawn cycle
            }

            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        GameObject ghostToSpawn = (Random.value < 0.5f || fireIsOut) ? redGhostPrefab : greenGhostPrefab;
        Vector3 spawnPosition = GetSpawnPositionOutsideCamera();
        Instantiate(ghostToSpawn, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetSpawnPositionOutsideCamera()
    {
        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        float randomDistance = Mathf.Max(verticalExtent, horizontalExtent) * 1.2f;
        Vector3 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnPosition = mainCamera.transform.position + randomDirection * randomDistance;
        spawnPosition.z = 0; 

        return spawnPosition;
    }
}
