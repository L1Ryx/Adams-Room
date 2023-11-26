using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    // Singleton instance
    public static HealthManager Instance { get; private set; }

    // Player hitpoints and maximum hitpoints
    public int hitPoints;
    public int maxHitPoints = 6;

    public Image[] hearts; // Array to store heart UI elements
    public Sprite fullHeartSprite; // Sprite for full heart
    public Sprite halfHeartSprite; // Sprite for half heart
    public Sprite emptyHeartSprite; // Sprite for empty heart

    public float invincibilityTime = 1.0f; // Duration of invincibility
    private bool isInvincible = false; // Flag to check if the player is currently invincible
    public SpriteRenderer playerSprite; // Reference to the player's sprite renderer


    private void Awake()
    {
        /// Ensure that there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep this object alive when loading new scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize hitpoints to max hitpoints at start
        hitPoints = maxHitPoints;
        UpdateHeartUI();

        // Find the player's sprite renderer
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
            if (playerSprite == null)
            {
                Debug.LogError("HealthManager: No SpriteRenderer found on the player.");
            }
        }
        else
        {
            Debug.LogError("HealthManager: Player not found.");
        }
    }

    // Method to reduce hitpoints
    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        hitPoints -= damage;
        hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints);
        UpdateHeartUI();

        if (hitPoints <= 0)
        {
            PlayerDied();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float endTime = Time.time + invincibilityTime;
        while (Time.time < endTime)
        {
            // Toggle the sprite's visibility
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        playerSprite.enabled = true; // Ensure sprite is visible after invincibility ends
        isInvincible = false;
    }

    // Method to heal the player
    public void Heal(int healAmount)
    {
        hitPoints += healAmount;
        hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints); // Ensures hitpoints do not exceed maxHitPoints
        UpdateHeartUI();
        // Additional logic for healing can be added here
    }

    private void PlayerDied()
    {
        AmbianceManager.Instance.FadeOutAndDestroyAll();
        EnemySpawnManager.Instance.SetFireIsOut(false);
        TimeManager.Instance.shouldShowResults = true;
        
    }

    // Additional methods for health management can be added here
    private void UpdateHeartUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartIndex = i * 2; // Each heart icon represents 2 hitpoints
            if (hitPoints > heartIndex + 1)
            {
                hearts[i].sprite = fullHeartSprite; // Set to full heart
            }
            else if (hitPoints > heartIndex)
            {
                hearts[i].sprite = halfHeartSprite; // Set to half heart
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite; // Set to empty heart
            }
        }
    }
}
