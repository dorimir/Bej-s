using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreOpacityController : MonoBehaviour
{
    [Header("Tiempo de activación/desactivación (segundos)")]
    public float startTime = 0f; // cuando empieza visible
    public float endTime = 5f;   // cuando deja de ser visible
    public float fadeDuration = 0.5f; // opcional, tiempo de fade

    private TextMeshProUGUI[] texts;

    private void Awake()
    {
        // Obtener todos los TMP children
        texts = GetComponentsInChildren<TextMeshProUGUI>(true);

        // Inicialmente ocultar todos
        SetAlpha(0f);
    }

    private void Start()
    {
        // Iniciar la coroutine que controla la opacidad
        StartCoroutine(OpacityRoutine());
    }

    private IEnumerator OpacityRoutine()
    {
        // Esperar hasta el momento de inicio
        yield return new WaitForSeconds(startTime);

        // Fade in
        if (fadeDuration > 0f)
            yield return StartCoroutine(FadeTo(1f, fadeDuration));
        else
            SetAlpha(1f);

        // Esperar hasta el momento de final
        float visibleTime = Mathf.Max(endTime - startTime - fadeDuration, 0f);
        yield return new WaitForSeconds(visibleTime);

        // Fade out
        if (fadeDuration > 0f)
            yield return StartCoroutine(FadeTo(0f, fadeDuration));
        else
            SetAlpha(0f);
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float[] initialAlphas = new float[texts.Length];
        for (int i = 0; i < texts.Length; i++)
            initialAlphas[i] = texts[i].alpha;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(initialAlphas[0], targetAlpha, t / duration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (texts == null) return;
        foreach (var tmp in texts)
        {
            tmp.alpha = alpha;
        }
    }
}