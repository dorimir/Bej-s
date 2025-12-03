using System.Collections;
using UnityEngine;

public class BowArrowBouncyAnimation : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform bowTransform;
    [SerializeField] private Transform arrowTransform;

    [Header("Configuración Animación")]
    [SerializeField] private float bounceDuration = 0.8f;
    [SerializeField] private float bounceHeight = 0.5f;
    [SerializeField] private AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 bowOriginalScale;
    private Vector3 arrowOriginalScale;
    private bool isAnimating = false;

    private void Start()
    {
        // Guardar escalas originales
        if (bowTransform != null)
            bowOriginalScale = bowTransform.localScale;
        if (arrowTransform != null)
            arrowOriginalScale = arrowTransform.localScale;

        // Reproducir animación al iniciar
        PlayBouncyAnimation();
    }

    public void PlayBouncyAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(BouncyAnimationCoroutine());
        }
    }

    private IEnumerator BouncyAnimationCoroutine()
    {
        isAnimating = true;
        float elapsedTime = 0f;

        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / bounceDuration;

            // Curva elástica para efecto bouncy
            float bounceValue = Mathf.Sin(progress * Mathf.PI * 2f) * (1f - progress);
            float scale = 1f + (bounceValue * 0.3f);

            // Aplicar escala bouncy
            if (bowTransform != null)
                bowTransform.localScale = bowOriginalScale * scale;

            if (arrowTransform != null)
                arrowTransform.localScale = arrowOriginalScale * scale;

            yield return null;
        }

        // Restaurar escalas originales
        if (bowTransform != null)
            bowTransform.localScale = bowOriginalScale;
        if (arrowTransform != null)
            arrowTransform.localScale = arrowOriginalScale;

        isAnimating = false;
    }
}