using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntentosUI : MonoBehaviour
{
    [Header("Referencia al contador")]
    [SerializeField] private ContadorDistancia contador;
    [SerializeField] private TextMeshProUGUI intentosText;

    private bool hasLoadedScene2 = false;

    void Update()
    {
        if (contador == null || intentosText == null) return;

        int currentTry = contador.GetCurrentTry();
        int remainingTries = contador.GetRemainingTries();
        int maxTries = currentTry + remainingTries - 1;

        // Si no quedan intentos, cargar Scene 2
        if (remainingTries <= 0 && !hasLoadedScene2)
        {
            hasLoadedScene2 = true;
            SceneManager.LoadScene("ResultsScene_Dora");
            return;
        }

        // Actualizar el texto
        if (remainingTries == 1)
        {
            intentosText.text = "¡Última oportunidad!";
        }
        else
        {
            intentosText.text = $"Intento {currentTry} / {maxTries}";
        }
    }
}