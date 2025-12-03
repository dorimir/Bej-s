using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public static bool showWinScreen = false;

    public GameObject winPanel;
    public GameObject losePanel; // AÑADE ESTO

    private void Start()
    {
        // Esconder ambos al inicio
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // Mostrar victoria
        if (showWinScreen)
        {
            if (winPanel != null) winPanel.SetActive(true);
            showWinScreen = false;
            Debug.Log("[WinScreenManager] Mostrando VICTORIA");
        }
        // Mostrar derrota
        else if (ContadorDistancia.loseCount >= 3)
        {
            if (losePanel != null) losePanel.SetActive(true);
            Debug.Log("[WinScreenManager] Mostrando DERROTA");
        }
    }

    public void LoadScene1()
    {
        ContadorDistancia.ResetGame(); // Resetear antes de volver
        SceneManager.LoadScene(1);
    }

    public void LoadScene0()
    {
        ContadorDistancia.ResetGame(); // Resetear antes de volver
        SceneManager.LoadScene(0);
    }
}