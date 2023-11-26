using UnityEngine;
using UnityEngine.UI;

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


    private void Awake()
    {
        // Ensure that there's only one instance of this class
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
    }

    // Method to reduce hitpoints
    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints); // Ensures hitpoints do not go below 0 or above max
        UpdateHeartUI();
        // Check for player death
        if (hitPoints <= 0)
        {
            PlayerDied();
        }
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
        TimeManager.Instance.shouldShowResults = true;
        Debug.Log("Player Died");
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
