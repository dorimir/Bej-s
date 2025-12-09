using System.Collections;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [Header("Objeto a mostrar (Sprite, UI, PNG, lo que sea)")]
    public GameObject spriteObject;

    [Header("Tiempos")]
    public float delayBeforeShow = 1f;
    public float timeVisible = 2f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Vector3 originalScale;

    private void Awake()
    {
        if (spriteObject == null)
        {
            Debug.LogError("[SpriteTimedAppearance] No se asignó ningún spriteObject.");
            return;
        }

        // Asegurar que tenga CanvasGroup
        canvasGroup = spriteObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = spriteObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        originalScale = spriteObject.transform.localScale;
        spriteObject.transform.localScale = Vector3.zero;
        spriteObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        // Espera sin afectar Time.timeScale
        yield return new WaitForSecondsRealtime(delayBeforeShow);

        spriteObject.SetActive(true);

        // Fade IN + Bounce
        yield return StartCoroutine(FadeInBounce());

        // Tiempo visible
        yield return new WaitForSecondsRealtime(timeVisible);

        // Fade OUT
        yield return StartCoroutine(FadeOut());

        spriteObject.SetActive(false);
    }

    private IEnumerator FadeInBounce()
    {
        float t = 0f;

        Vector3 overshoot = originalScale * 1.10f; // pequeño bounce
        Vector3 undershoot = originalScale * 0.95f;

        // Fade + escalar hasta overshoot
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / fadeDuration;

            canvasGroup.alpha = p;
            spriteObject.transform.localScale = Vector3.Lerp(Vector3.zero, overshoot, p);

            yield return null;
        }

        // Reducir al tamaño real para el efecto bounce
        t = 0f;
        while (t < 0.1f)
        {
            t += Time.unscaledDeltaTime;
            float p = t / 0.1f;

            spriteObject.transform.localScale = Vector3.Lerp(overshoot, undershoot, p);

            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.unscaledDeltaTime;
            float p = t / 0.1f;

            spriteObject.transform.localScale = Vector3.Lerp(undershoot, originalScale, p);

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, p);

            yield return null;
        }
    }
}
