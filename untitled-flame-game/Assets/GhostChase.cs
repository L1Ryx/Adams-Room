using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class GhostChase : MonoBehaviour
{
    public float speed = 5f;
    public float noiseStrength = 0.5f;
    public float noiseSpeed = 2f;

    private GameObject player;
    private Vector2 originalPosition;
    private float noiseOffset;

    public int maxHitPoints = 3;
    private int currentHitPoints;
    public float knockbackStrength = 2f;
    public float knockbackDuration = 0.5f;
    private bool isKnockedBack = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Light2D ghostLight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPosition = transform.position;
        noiseOffset = Random.Range(0f, 1000f); // Random offset for noise
        rb = GetComponent<Rigidbody2D>();

        currentHitPoints = maxHitPoints;
        Transform ghostVisualsTransform = transform.Find("GhostVisuals");
        if (ghostVisualsTransform != null)
        {
            spriteRenderer = ghostVisualsTransform.GetComponent<SpriteRenderer>();
            ghostLight = ghostVisualsTransform.GetComponentInChildren<Light2D>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("GhostChase: No SpriteRenderer found on GhostVisuals child.");
        }
        if (ghostLight == null)
        {
            Debug.LogError("GhostChase: No Light2D component found on GhostVisuals child.");
        }
    }

    private void FixedUpdate()
    {
        if (player != null && !isKnockedBack)
        {
            Vector2 targetPosition = player.transform.position;
            Vector2 currentPosition = transform.position;
            Vector2 moveDirection = (targetPosition - currentPosition).normalized;
            float noise = Mathf.PerlinNoise(Time.time * noiseSpeed + noiseOffset, 0f) * 2f - 1f; // Noise value between -1 and 1

            // Apply noise to movement
            moveDirection += new Vector2(moveDirection.y, -moveDirection.x) * noise * noiseStrength;
            rb.velocity = moveDirection * speed;

            // Update the facing direction based on the movement
            UpdateFacingDirection(currentPosition, targetPosition);
        }
    }

    private void UpdateFacingDirection(Vector2 currentPosition, Vector2 targetPosition)
    {
        bool isMovingRight = targetPosition.x > currentPosition.x;
        if (isMovingRight)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager.Instance.TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Slash") && !isKnockedBack)
        {
            TakeDamage(1);
            StartCoroutine(ApplyKnockback(collision.transform.position));
        }
    }

    private IEnumerator ApplyKnockback(Vector3 sourcePosition)
    {
        Vector2 knockbackDirection = (transform.position - sourcePosition).normalized;
        isKnockedBack = true;

        // Apply a strong impulse force
        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;
        rb.velocity = Vector2.zero; // Reset velocity after knockback
    }




    private void TakeDamage(int damage)
    {
        currentHitPoints -= damage;

        if (currentHitPoints <= 0)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        if (spriteRenderer == null || ghostLight == null)
        {
            Debug.LogError("GhostChase: Missing components for fade out.");
            yield break;
        }

        float fadeDuration = 1f;
        float fadeSpeed = 1 / fadeDuration;
        float alpha = spriteRenderer.color.a;
        float originalIntensity = ghostLight.intensity;

        Color originalColor = spriteRenderer.color;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            ghostLight.intensity = Mathf.Lerp(originalIntensity, 0, 1 - alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
