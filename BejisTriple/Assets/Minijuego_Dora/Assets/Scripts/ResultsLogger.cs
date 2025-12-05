using UnityEngine;
using TMPro;

public class ResultsLogger : MonoBehaviour
{
    private static float[] attemptDistances = new float[3];
    private static int attemptsMade = 0;
    private static bool victoryAchieved = false;
    private static int victoryAttempt = -1; // número de intento en el que se ganó

    [Header("UI References (solo en ResultsScene_Dora)")]
    [SerializeField] private TextMeshProUGUI attempt1Text;
    [SerializeField] private TextMeshProUGUI attempt2Text;
    [SerializeField] private TextMeshProUGUI attempt3Text;

    // Guardar resultado de un intento
    public static void RegisterAttempt(int attemptNumber, float distance, bool isVictory = false)
    {
        if (attemptNumber < 1 || attemptNumber > 3) return;

        attemptDistances[attemptNumber - 1] = distance;
        attemptsMade = Mathf.Max(attemptsMade, attemptNumber);

        if (isVictory)
        {
            victoryAchieved = true;
            victoryAttempt = attemptNumber;
        }

        Debug.Log($"[ResultsLogger] Intento {attemptNumber}: {distance.ToString("F1")}m (victoria={isVictory})");
    }

    public static void ResetResults()
    {
        for (int i = 0; i < attemptDistances.Length; i++)
            attemptDistances[i] = 0f;

        attemptsMade = 0;
        victoryAchieved = false;
        victoryAttempt = -1;
    }

    private void Start()
    {
        ShowResultsInUI();
    }

    private void ShowResultsInUI()
    {
        float[] results = attemptDistances;

        Debug.Log($"[ResultsLogger] Resultados finales -> 1:{FormatForDisplay(1, results[0])}, 2:{FormatForDisplay(2, results[1])}, 3:{FormatForDisplay(3, results[2])}");

        if (attempt1Text != null)
            attempt1Text.text = attemptsMade >= 1 ? FormatForDisplay(1, results[0]) : "-";

        if (attempt2Text != null)
            attempt2Text.text = attemptsMade >= 2 ? FormatForDisplay(2, results[1]) : "-";

        if (attempt3Text != null)
            attempt3Text.text = attemptsMade >= 3 ? FormatForDisplay(3, results[2]) : "-";
    }

    // Si fue victoria en ese intento, mostrar texto fijo "70m". Si no, mostrar distancia con 1 decimal.
    private string FormatForDisplay(int attemptNumber, float distance)
    {
        if (victoryAchieved && attemptNumber == victoryAttempt)
            return "70m";

        return distance.ToString("F1") + "m";
    }
}