using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContadorDistancia : MonoBehaviour
{
    [Header("Referencias")]
    public Transform arrow;
    public Transform startPoint;
    public TextMeshProUGUI distanceText;

    [Header("Efectos")]
    [SerializeField] private GameEffectsManager effectsManager;

    private void Awake()
    {
        // Crear GameEffectsManager automáticamente si no existe
        if (effectsManager == null)
        {
            GameObject effectsObj = new GameObject("GameEffectsManager");
            effectsManager = effectsObj.AddComponent<GameEffectsManager>();
            Debug.Log("[ContadorDistancia] GameEffectsManager creado automáticamente");
        }
    }

    [Header("Configuración")]
    public bool showMeters = true;
    public int decimalPlaces = 1;

    [Header("Meta de Distancia")]
    public float targetDistance = 0f;
    public string nextSceneName = "";

    [Header("Intentos")]
    public int maxTries = 3;

    // Variables estáticas 
    public static int loseCount = 0;
    public static bool showWinScreen = false;
    public static List<GameObject> persistentArrows = new List<GameObject>();

    // Variables internas
    private float maxDistance = 0f;
    private Vector3 initialPosition;
    private bool hasCollided = false;
    private bool hasWon = false;

    // Propiedades públicas
    public int GetCurrentTry() => loseCount + 1;
    public int GetRemainingTries() => maxTries - loseCount;
    public float GetMaxDistance() => maxDistance;

    private void Start()
    {
        initialPosition = startPoint != null ? startPoint.position : arrow.position;
        UpdateDistanceText(0f);
        hasCollided = false;
        hasWon = false;

        Debug.Log($"[ContadorDistancia] Inicio - Intento {GetCurrentTry()}/{maxTries}, loseCount: {loseCount}");
    }

    private void Update()
    {
        if (arrow == null || hasCollided) return;

        float currentDistance = Mathf.Abs((arrow.position.x - initialPosition.x) / 20f);
        if (currentDistance > maxDistance)
        {
            maxDistance = currentDistance;
            UpdateDistanceText(maxDistance);
            CheckWinCondition();
        }

        // Debug: simular colisión con L
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnArrowCollided();
            if (SoundController.Instance != null)
                SoundController.Instance.PlayCollisionGround();
        }
    }

    private void UpdateDistanceText(float distance)
    {
        if (distanceText == null) return;
        distanceText.text = distance.ToString("F" + decimalPlaces) + (showMeters ? "m" : "");
    }

    private void CheckWinCondition()
    {
        if (targetDistance <= 0f || hasWon) return;
        if (maxDistance >= targetDistance)
        {
            hasWon = true;
            OnWin();
        }
    }
    private void OnWin()
    {
        Debug.Log($"[ContadorDistancia] ¡VICTORIA! Distancia: {maxDistance:F1}m");

        // Registrar el intento con la distancia real alcanzada
        ResultsLogger.RegisterAttempt(GetCurrentTry(), maxDistance);

        if (effectsManager != null && arrow != null)
        {
            effectsManager.PlayConfetti(arrow.position);
        }
        if (SoundController.Instance != null)
        {
            SoundController.Instance.PlayWin();
        }
        WinScreenManager.showWinScreen = true;
        StartCoroutine(LoadWinSceneDelayed());
    }

    private IEnumerator LoadWinSceneDelayed()
    {
        yield return new WaitForSeconds(1f);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene("ResultsScene_Dora");
    }

    public void OnArrowCollided()
    {
        if (hasCollided)
        {
            return;
        }

        hasCollided = true;

        // Congelar flecha
        if (arrow != null)
        {
            arrow.SetParent(null);
            arrow.gameObject.name = $"PersistentArrow_{loseCount + 1}";

            // 🚨 MUY IMPORTANTE: evitar que la flecha tenga tag Player
            arrow.tag = "Untagged";

            DontDestroyOnLoad(arrow.gameObject);
            persistentArrows.Add(arrow.gameObject);


            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Dentro de OnArrowCollided(), después de calcular maxDistance
            ResultsLogger.RegisterAttempt(GetCurrentTry(), GetMaxDistance());

            Collider col = arrow.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            AudioListener listener = arrow.GetComponent<AudioListener>();
            if (listener != null) Destroy(listener);

            Debug.Log($"[ContadorDistancia] Flecha congelada: {arrow.gameObject.name}");
        }

        if (!hasWon && effectsManager != null && arrow != null)
        {
            Debug.Log("[ContadorDistancia] Llamando a ZoomToArrow...");
            effectsManager.ZoomToArrow(arrow);
        }

        if (!hasWon)
        {
            loseCount++;
            Debug.Log($"[ContadorDistancia] LOSE COUNT: {loseCount}/{maxTries}");
            if (SoundController.Instance != null)
            {
                SoundController.Instance.PlayLose();
            }
            if (loseCount >= maxTries)
            {
                Debug.Log("[ContadorDistancia] ===== GAME OVER - 3 INTENTOS AGOTADOS =====");
                StartCoroutine(LoadLoseSceneDelayed());
            }
            else
            {
                StartCoroutine(ReloadSceneForNextTry());
            }
        }
    }

    private IEnumerator ReloadSceneForNextTry()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("TiroConArco");
    }

    private IEnumerator LoadLoseSceneDelayed()
    {
        // Esperar más tiempo para que se complete el zoom
        yield return new WaitForSeconds(3f);
        //ContadorDistancia.ResetGame();
        SceneManager.LoadScene("ResultsScene_Dora");
    }

    public static void ResetGame()
    {
        Debug.Log("[ContadorDistancia] ===== RESET COMPLETO =====");
        loseCount = 0;
        WinScreenManager.showWinScreen = false;

        foreach (GameObject arrow in persistentArrows)
        {
            if (arrow != null) Destroy(arrow);
        }
        persistentArrows.Clear();

        CameraCutSceneController.ResetCutScene();

        // 🔑 Resetear también los resultados
        ResultsLogger.ResetResults();
    }
}
