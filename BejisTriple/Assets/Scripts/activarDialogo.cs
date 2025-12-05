using UnityEngine;
using UnityEngine.EventSystems;

public class activarDialogo : MonoBehaviour, IPointerClickHandler
{
    [Header("Referencias")]
    [Tooltip("El sprite que se revelará al acercarse (ej: icono encima del NPC)")]
    public SpriteRenderer hintSprite;

    [Tooltip("Transform del jugador")]
    public Transform player;

    [Tooltip("ScriptableObject con el diálogo del NPC")]
    public dialogoSO dialogo;

    [Header("Detección")]
    [Tooltip("Radio de detección para revelar el sprite")]
    public float detectionRadius = 5f;

    [Header("Animación")]
    public float bounceInDuration = 0.5f;
    public float fadeOutDuration = 0.3f;
    public float bounceOvershoot = 1.2f;

    // Variables privadas
    private bool isVisible = false;
    private bool isAnimating = false;
    private float animationTimer = 0f;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        if (hintSprite != null)
        {
            originalScale = hintSprite.transform.localScale;
            originalColor = hintSprite.color;

            // Comenzar oculto
            hintSprite.transform.localScale = Vector3.zero;
            Color c = hintSprite.color;
            c.a = 0f;
            hintSprite.color = c;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (hintSprite == null || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool shouldBeVisible = distance <= detectionRadius;

        if (shouldBeVisible && !isVisible && !isAnimating)
            StartBounceIn();
        else if (!shouldBeVisible && isVisible && !isAnimating)
            StartFadeOut();

        if (isAnimating)
        {
            animationTimer += Time.deltaTime;
            if (isVisible) UpdateBounceIn();
            else UpdateFadeOut();
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
            hintSprite.transform.localScale = originalScale;
            Color c = hintSprite.color;
            c.a = originalColor.a;
            hintSprite.color = c;
            isAnimating = false;
            return;
        }

        float scale = ElasticEaseOut(progress, bounceOvershoot);
        hintSprite.transform.localScale = originalScale * scale;

        Color color = hintSprite.color;
        color.a = originalColor.a * progress;
        hintSprite.color = color;
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
            hintSprite.transform.localScale = Vector3.zero;
            Color c = hintSprite.color;
            c.a = 0f;
            hintSprite.color = c;
            isAnimating = false;
            return;
        }

        float eased = 1f - (1f - progress) * (1f - progress);
        hintSprite.transform.localScale = originalScale * (1f - eased);

        Color color = hintSprite.color;
        color.a = originalColor.a * (1f - progress);
        hintSprite.color = color;
    }

    float ElasticEaseOut(float t, float overshoot)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        float p = 0.3f;
        float s = p / 4f;
        return overshoot * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) + 1f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Al hacer clic en el NPC visible, inicia el diálogo
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isVisible) // Solo si el hint está revelado
        {
            managerDialogo manager = FindObjectOfType<managerDialogo>();
            if (manager != null)
                manager.IniciarDialogo(dialogo);
        }
    }
}