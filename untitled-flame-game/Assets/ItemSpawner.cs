using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    public GameObject logPrefab; // Drag your item prefab here in the inspector
    public float spawnInterval = 3.0f; // Time between each spawn in seconds

    private Vector2 topLeft = new Vector2(-13.5f, 8.5f);
    private Vector2 bottomRight = new Vector2(14.5f, -5.5f);
    private Vector2[,] gridPositions;
    private bool[,] isOccupied; // To keep track of occupied grid positions

    public int noSpawnZoneWidth = 5;  // 'a' dimension
    public int noSpawnZoneHeight = 5; // 'b' dimension

    void Awake()
    {
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize grid
        int gridWidth = Mathf.RoundToInt(Mathf.Abs(topLeft.x - bottomRight.x)) + 1;
        int gridHeight = Mathf.RoundToInt(Mathf.Abs(topLeft.y - bottomRight.y)) + 1;
        gridPositions = new Vector2[gridWidth, gridHeight];
        isOccupied = new bool[gridWidth, gridHeight]; // Initialize isOccupied array

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                gridPositions[x, y] = new Vector2(topLeft.x + x, topLeft.y - y);
                isOccupied[x, y] = false; // Initialize all positions as not occupied
            }
        }

        // Start item spawning
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Find a free grid position
            int randomX, randomY;
            bool isInNoSpawnZone;

            do
            {
                randomX = Random.Range(0, gridPositions.GetLength(0));
                randomY = Random.Range(0, gridPositions.GetLength(1));

                // Calculate the boundaries of the no-spawn zone
                int startX = (gridPositions.GetLength(0) - noSpawnZoneWidth) / 2;
                int endX = startX + noSpawnZoneWidth;
                int startY = (gridPositions.GetLength(1) - noSpawnZoneHeight) / 2;
                int endY = startY + noSpawnZoneHeight;

                isInNoSpawnZone = (randomX >= startX && randomX < endX) && (randomY >= startY && randomY < endY);

                // Keep looking while the position is occupied or within the no-spawn zone
            } while (isOccupied[randomX, randomY] || isInNoSpawnZone);

            // Mark the chosen position as occupied
            isOccupied[randomX, randomY] = true;

            // Get the spawn position
            Vector2 spawnPosition = gridPositions[randomX, randomY];

            // Instantiate the item at the chosen position
            GameObject newItem = Instantiate(logPrefab, spawnPosition, Quaternion.identity);
            Log logScript = newItem.GetComponent<Log>();
            logScript.SetGridPosition(randomX, randomY);

            // Optionally, make the spawned item a child of this spawner for easier management in the hierarchy
            newItem.transform.parent = transform;
        }
    }



    public void MarkPositionAsFree(int x, int y)
    {
        isOccupied[x, y] = false; // Mark this grid position as free
    }
}
