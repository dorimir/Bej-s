using UnityEngine;
using UnityEngine.SceneManagement;

public class introSceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "TiroConArco";

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1.2f;

    private bool isChangingScene = false;

    private void Start()
    {
        // Empieza en negro
        fadeCanvasGroup.alpha = 1f;
        StartCoroutine(FadeInRoutine());
    }

    private void Update()
    {
        if (!isChangingScene && Input.GetKeyDown(KeyCode.Space))
        {
            isChangingScene = true;
            StartCoroutine(FadeOutAndChangeScene());
        }
    }


    // ---------------------
    // --- FADE IN -------
    // ---------------------
    private System.Collections.IEnumerator FadeInRoutine()
    {
        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (t / fadeInDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }


    // ---------------------
    // --- FADE OUT --------
    // ---------------------
    private System.Collections.IEnumerator FadeOutAndChangeScene()
    {
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeOutDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.LoadScene(sceneToLoad);
    }
}