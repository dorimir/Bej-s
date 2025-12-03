using UnityEngine;

public class ProximitySpriteReveal : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The sprite renderer to show/hide")]
    public SpriteRenderer targetSprite;

    [Tooltip("The player transform to track")]
    public Transform player;

    [Header("Detection Settings")]
    [Tooltip("Radius within which the sprite appears")]
    public float detectionRadius = 5f;

    [Header("Animation Settings")]
    [Tooltip("Duration of the bounce-in animation")]
    public float bounceInDuration = 0.5f;

    [Tooltip("Duration of the fade-out animation")]
    public float fadeOutDuration = 0.3f;

    [Tooltip("Bounce overshoot amount (higher = more bounce)")]
    public float bounceOvershoot = 1.2f;

    // Private variables
    private bool isVisible = false;
    private bool isAnimating = false;
    private float animationTimer = 0f;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        // Store original values
        if (targetSprite != null)
        {
            originalScale = targetSprite.transform.localScale;
            originalColor = targetSprite.color;

            // Start hidden
            targetSprite.transform.localScale = Vector3.zero;
            Color c = targetSprite.color;
            c.a = 0f;
            targetSprite.color = c;
        }
        else
        {
            Debug.LogWarning("Target Sprite not assigned!");
        }

        if (player == null)
        {
            Debug.LogWarning("Player not assigned! Trying to find GameObject with 'Player' tag...");
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (targetSprite == null || player == null) return;

        // Check distance to player
        float distance = Vector2.Distance(transform.position, player.position);
        bool shouldBeVisible = distance <= detectionRadius;

        // Start animation if state should change
        if (shouldBeVisible && !isVisible && !isAnimating)
        {
            StartBounceIn();
        }
        else if (!shouldBeVisible && isVisible && !isAnimating)
        {
            StartFadeOut();
        }

        // Update ongoing animation
        if (isAnimating)
        {
            animationTimer += Time.deltaTime;

            if (isVisible)
            {
                UpdateBounceIn();
            }
            else
            {
                UpdateFadeOut();
            }
        }
    }

    void StartBounceIn()
    {
        isAnimating = true;
        isVisible = true;
        animationTimer = 0f;
    }

    void UpdateBounceIn()
    {
        float progress = animationTimer / bounceInDuration;

        if (progress >= 1f)
        {
            // Animation complete
            targetSprite.transform.localScale = originalScale;
            Color c = targetSprite.color;
            c.a = originalColor.a;
            targetSprite.color = c;
            isAnimating = false;
            return;
        }

        // Elastic ease out for bounce effect
        float scale = ElasticEaseOut(progress, bounceOvershoot);
        targetSprite.transform.localScale = originalScale * scale;

        // Fade in alpha
        Color color = targetSprite.color;
        color.a = originalColor.a * progress;
        targetSprite.color = color;
    }

    void StartFadeOut()
    {
        isAnimating = true;
        isVisible = false;
        animationTimer = 0f;
    }

    void UpdateFadeOut()
    {
        float progress = animationTimer / fadeOutDuration;

        if (progress >= 1f)
        {
            // Animation complete
            targetSprite.transform.localScale = Vector3.zero;
            Color c = targetSprite.color;
            c.a = 0f;
            targetSprite.color = c;
            isAnimating = false;
            return;
        }

        // Ease out for smooth fade
        float eased = 1f - (1f - progress) * (1f - progress);

        // Scale down
        targetSprite.transform.localScale = originalScale * (1f - eased);

        // Fade out alpha
        Color color = targetSprite.color;
        color.a = originalColor.a * (1f - progress);
        targetSprite.color = color;
    }

    // Elastic ease out function for bouncy effect
    float ElasticEaseOut(float t, float overshoot)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;

        float p = 0.3f;
        float s = p / 4f;

        return overshoot * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) + 1f;
    }

    // Visualize detection radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}