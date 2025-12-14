using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public static bool showWinScreen = false;

    public GameObject winPanel;
    public GameObject losePanel;

    private void OnEnable()
    {
        Update();
    }

    // Llamar a esta función siempre que haya un cambio de estado
    private void Update()
    {
        // Mostrar derrota si el jugador ha perdido y el panel aún no está activo
        if (ContadorDistancia.loseCount >= 3 && !losePanel.activeSelf)
        {
            losePanel.SetActive(true);
            winPanel.SetActive(false);
            Debug.Log("[WinScreenManager] Mostrando DERROTA");
        }

        // Mostrar victoria si toca
        if (showWinScreen && !winPanel.activeSelf)
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);
            showWinScreen = false;
            Debug.Log("[WinScreenManager] Mostrando VICTORIA");
        }
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
    }

    public void LoadScene1()
    {
        ContadorDistancia.ResetGame();
        SceneManager.LoadScene("TiroConArco");
    }

    public void LoadScene0()
    {
        ContadorDistancia.ResetGame();

        // 🔹 Destruir SoundController si existe
        if (SoundController.Instance != null)
        {
            Destroy(SoundController.Instance.gameObject);
            SoundController.Instance = null;
        }
        GameManager.Instance.minijuegoCompletado(4); //sigleton
        SceneManager.LoadScene("Rio");
    }
}