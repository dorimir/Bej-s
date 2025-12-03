using UnityEngine;

public class CameraCutSceneController : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private GameObject cutSceneCamera;
    [SerializeField] private GameObject gameplayCamera;
    [SerializeField] private GameObject camara2Parent; // Opcional: referencia directa al padre

    [Header("Duración de la cinemática")]
    [SerializeField] private float cutSceneDuration = 5f;

    [Header("Posición inicial de la cinemática")]
    [SerializeField] private Vector3 cutSceneStartPosition = new Vector3(1155f, 20.104f, -44.41f);
    [SerializeField] private Vector3 cutSceneStartRotation = new Vector3(0f, 0f, 0f);

    // Variable ESTÁTICA para saber si ya se reprodujo EN ESTA SESIÓN DE JUEGO
    private static bool hasPlayedThisSession = false;

    private void Start()
    {
        // Solo reproducir si es el primer intento (loseCount == 0)
        if (ContadorDistancia.loseCount == 0 && !hasPlayedThisSession)
        {
            PlayCutScene();
            hasPlayedThisSession = true;
            Debug.Log("[CutScene] Reproduciendo cutscene - Primera vez (loseCount = 0)");
        }
        else
        {
            ActivateGameplayCamera();
            Debug.Log($"[CutScene] Cutscene omitida - loseCount: {ContadorDistancia.loseCount}, hasPlayed: {hasPlayedThisSession}");
        }
    }

    private void PlayCutScene()
    {
        // Activar Camara2 padre si está asignado
        if (camara2Parent != null)
        {
            camara2Parent.SetActive(true);
        }

        if (cutSceneCamera != null)
        {
            cutSceneCamera.transform.position = cutSceneStartPosition;
            cutSceneCamera.transform.eulerAngles = cutSceneStartRotation;
            cutSceneCamera.SetActive(true);

            // Si no hay referencia al padre, intentar activarlo automáticamente
            if (camara2Parent == null && cutSceneCamera.transform.parent != null)
            {
                cutSceneCamera.transform.parent.gameObject.SetActive(true);
            }
        }

        if (gameplayCamera != null)
        {
            gameplayCamera.SetActive(false);

            // Desactivar también el padre si existe
            if (gameplayCamera.transform.parent != null)
            {
                gameplayCamera.transform.parent.gameObject.SetActive(false);
            }
        }

        Invoke(nameof(ActivateGameplayCamera), cutSceneDuration);
    }

    private void ActivateGameplayCamera()
    {
        // Desactivar Camara2 padre si está asignado
        if (camara2Parent != null)
        {
            camara2Parent.SetActive(false);
        }

        if (cutSceneCamera != null)
        {
            cutSceneCamera.SetActive(false);

            // Si no hay referencia al padre, intentar desactivarlo automáticamente
            if (camara2Parent == null && cutSceneCamera.transform.parent != null)
            {
                cutSceneCamera.transform.parent.gameObject.SetActive(false);
            }
        }

        if (gameplayCamera != null)
        {
            gameplayCamera.SetActive(true);

            // Activar también el padre
            if (gameplayCamera.transform.parent != null)
            {
                gameplayCamera.transform.parent.gameObject.SetActive(true);
            }
        }
    }

    // Método estático para resetear la cutscene (cuando se reinicia el juego)
    public static void ResetCutScene()
    {
        hasPlayedThisSession = false;
        Debug.Log("[CutScene] Cutscene reseteada - Se reproducirá en el próximo juego");
    }
}