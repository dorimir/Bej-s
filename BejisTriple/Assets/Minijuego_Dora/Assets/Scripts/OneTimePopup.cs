using System.Collections;
using UnityEngine;

public class OneTimePopup : MonoBehaviour
{
    [Header("Identificador Único")]
    [Tooltip("ID único para este popup. Cambia esto para cada popup diferente.")]
    public string popupID = "Tutorial_1";

    [Header("Objeto a mostrar")]
    public GameObject spriteObject;

    [Header("Tiempos")]
    public float delayBeforeShow = 1f;
    public float timeVisible = 2f;
    public float fadeDuration = 0.5f;

    [Header("Debug")]
    [Tooltip("Marcar para forzar que se muestre aunque ya se haya visto antes")]
    public bool forceShowForTesting = false;

    private CanvasGroup canvasGroup;
    private Vector3 originalScale;

    private void Awake()
    {
        if (spriteObject == null)
        {
            Debug.LogError("[OneTimePopup] No se asignó ningún spriteObject.");
            enabled = false;
            return;
        }

        // Verificar INMEDIATAMENTE si ya se mostró
        bool alreadyShown = PopupManager.HasBeenShown(popupID);

        Debug.Log($"[OneTimePopup] '{popupID}' - Ya mostrado: {alreadyShown}, Force testing: {forceShowForTesting}");

        if (alreadyShown && !forceShowForTesting)
        {
            // Ya se mostró antes, desactivar
            Debug.Log($"[OneTimePopup] '{popupID}' ya fue mostrado, ocultando...");
            spriteObject.SetActive(false);
            enabled = false;
            return;
        }

        // Primera vez, configurar para mostrar
        Debug.Log($"[OneTimePopup] '{popupID}' se va a mostrar");

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
        // Si llegamos aquí es porque NO se ha mostrado antes
        Debug.Log($"[OneTimePopup] Mostrando '{popupID}' por primera vez");
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        yield return new WaitForSecondsRealtime(delayBeforeShow);
        spriteObject.SetActive(true);

        yield return StartCoroutine(FadeInBounce());
        yield return new WaitForSecondsRealtime(timeVisible);
        yield return StartCoroutine(FadeOut());

        spriteObject.SetActive(false);

        // ✨ Marcar como mostrado AL FINAL de la secuencia
        PopupManager.MarkAsShown(popupID);
        Debug.Log($"[OneTimePopup] '{popupID}' terminó de mostrarse, marcado como visto");
    }

    private IEnumerator FadeInBounce()
    {
        float t = 0f;
        Vector3 overshoot = originalScale * 1.10f;
        Vector3 undershoot = originalScale * 0.95f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / fadeDuration;
            canvasGroup.alpha = p;
            spriteObject.transform.localScale = Vector3.Lerp(Vector3.zero, overshoot, p);
            yield return null;
        }

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

    // ✨ MÉTODOS ÚTILES PARA DEBUG

    /// <summary>
    /// Resetea ESTE popup para que vuelva a mostrarse
    /// </summary>
    [ContextMenu("Reset This Popup")]
    public void ResetThisPopup()
    {
        PopupManager.ResetPopup(popupID);
        Debug.Log($"[OneTimePopup] Popup '{popupID}' reseteado. Se mostrará de nuevo.");
    }

    /// <summary>
    /// Resetea TODOS los popups del juego
    /// </summary>
    [ContextMenu("Reset ALL Popups")]
    public void ResetAllPopupsMenu()
    {
        PopupManager.ResetAllPopups();
        Debug.Log("[OneTimePopup] TODOS los popups han sido reseteados.");
    }
}