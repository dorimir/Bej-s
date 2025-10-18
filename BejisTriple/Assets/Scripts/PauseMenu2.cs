using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu2 : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject warningPanel;

    [Header("Pause Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button leaveButton;

    [Header("Warning Panel Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [Header("Player Reference")]
    [SerializeField] private GameObject player;

    private bool isPaused = false;
    private MonoBehaviour[] playerScripts;

    void Start()
    {
        // Hide panels at start
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (warningPanel != null) warningPanel.SetActive(false);

        // Get all player scripts to disable/enable them
        if (player != null)
        {
            playerScripts = player.GetComponents<MonoBehaviour>();
        }

        // Setup button listeners
        if (continueButton != null) continueButton.onClick.AddListener(ContinueGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (leaveButton != null) leaveButton.onClick.AddListener(ShowWarning);
        if (confirmButton != null) confirmButton.onClick.AddListener(ConfirmLeave);
        if (cancelButton != null) cancelButton.onClick.AddListener(CancelLeave);
    }

    void Update()
    {
        // Press ESC to toggle pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ContinueGame();
            else
                PauseGame();
        }
    }

    // Public method to open pause menu from any button
    public void OpenPauseMenu()
    {
        Debug.Log("OpenPauseMenu called!");
        PauseGame();
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame called!");
        isPaused = true;
        Time.timeScale = 0f;

        // Disable player movement scripts
        DisablePlayerMovement();

        // Show pause menu
        if (pauseMenuPanel != null)
        {
            Debug.Log("Activating pause menu panel");
            pauseMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("pauseMenuPanel is NULL! Please assign it in Inspector!");
        }

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Enable player movement scripts
        EnablePlayerMovement();

        // Hide all panels
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (warningPanel != null) warningPanel.SetActive(false);

        // Lock cursor (for FPS games)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        // Placeholder for settings
        Debug.Log("Settings button clicked - Add your settings functionality here");
    }

    public void ShowWarning()
    {
        // Hide pause menu and show warning
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (warningPanel != null) warningPanel.SetActive(true);
    }

    public void ConfirmLeave()
    {
        // Reset time scale before loading scene
        Time.timeScale = 1f;

        // Load main menu scene
        SceneManager.LoadScene("MainMenuScene");
    }

    public void CancelLeave()
    {
        // Go back to pause menu
        if (warningPanel != null) warningPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    private void DisablePlayerMovement()
    {
        if (playerScripts == null) return;

        foreach (var script in playerScripts)
        {
            // Don't disable this script itself
            if (script != this && script != null)
            {
                script.enabled = false;
            }
        }
    }

    private void EnablePlayerMovement()
    {
        if (playerScripts == null) return;

        foreach (var script in playerScripts)
        {
            if (script != this && script != null)
            {
                script.enabled = true;
            }
        }
    }
}