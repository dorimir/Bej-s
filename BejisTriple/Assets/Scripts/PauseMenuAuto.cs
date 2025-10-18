using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuAuto : MonoBehaviour
{
    private GameObject pauseMenuPanel;
    private GameObject warningPanel;
    private GameObject player;
    private MonoBehaviour[] playerScripts;
    private bool isPaused = false;

    void Start()
    {
        CreatePauseMenuUI();

        // Try to find player automatically
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScripts = player.GetComponents<MonoBehaviour>();
        }
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

    private void CreatePauseMenuUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("PauseMenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Pause Menu Panel
        pauseMenuPanel = CreatePanel(canvasObj.transform, "PauseMenuPanel", new Color(0, 0, 0, 0.8f));

        // Create title text
        CreateText(pauseMenuPanel.transform, "PAUSED", new Vector2(0, 150), 48);

        // Create buttons
        GameObject continueBtn = CreateButton(pauseMenuPanel.transform, "Continue", new Vector2(0, 50));
        continueBtn.GetComponent<Button>().onClick.AddListener(ContinueGame);

        GameObject settingsBtn = CreateButton(pauseMenuPanel.transform, "Settings", new Vector2(0, -20));
        settingsBtn.GetComponent<Button>().onClick.AddListener(OpenSettings);

        GameObject leaveBtn = CreateButton(pauseMenuPanel.transform, "Leave Game", new Vector2(0, -90));
        leaveBtn.GetComponent<Button>().onClick.AddListener(ShowWarning);

        pauseMenuPanel.SetActive(false);

        // Create Warning Panel
        warningPanel = CreatePanel(canvasObj.transform, "WarningPanel", new Color(0.1f, 0.1f, 0.1f, 0.9f));

        CreateText(warningPanel.transform, "Are you sure you want to leave?", new Vector2(0, 80), 32);
        CreateText(warningPanel.transform, "Progress may be lost", new Vector2(0, 30), 20);

        GameObject confirmBtn = CreateButton(warningPanel.transform, "Yes, Leave", new Vector2(-100, -50));
        confirmBtn.GetComponent<Button>().onClick.AddListener(ConfirmLeave);
        confirmBtn.GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f);

        GameObject cancelBtn = CreateButton(warningPanel.transform, "Cancel", new Vector2(100, -50));
        cancelBtn.GetComponent<Button>().onClick.AddListener(CancelLeave);

        warningPanel.SetActive(false);
    }

    private GameObject CreatePanel(Transform parent, string name, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        Image img = panel.AddComponent<Image>();
        img.color = color;

        return panel;
    }

    private GameObject CreateButton(Transform parent, string text, Vector2 position)
    {
        GameObject buttonObj = new GameObject("Button_" + text);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 60);

        Image img = buttonObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 1f);

        Button btn = buttonObj.AddComponent<Button>();
        ColorBlock colors = btn.colors;
        colors.normalColor = new Color(0.2f, 0.6f, 1f);
        colors.highlightedColor = new Color(0.3f, 0.7f, 1f);
        colors.pressedColor = new Color(0.1f, 0.5f, 0.9f);
        btn.colors = colors;

        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        Text textComp = textObj.AddComponent<Text>();
        textComp.text = text;
        textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComp.fontSize = 24;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleCenter;

        return buttonObj;
    }

    private void CreateText(Transform parent, string content, Vector2 position, int fontSize)
    {
        GameObject textObj = new GameObject("Text_" + content.Substring(0, Mathf.Min(10, content.Length)));
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(800, 100);

        Text text = textObj.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        DisablePlayerMovement();

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        EnablePlayerMovement();

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (warningPanel != null) warningPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked - Add your settings functionality here");
    }

    public void ShowWarning()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (warningPanel != null) warningPanel.SetActive(true);
    }

    public void ConfirmLeave()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void CancelLeave()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    private void DisablePlayerMovement()
    {
        if (playerScripts == null) return;

        foreach (var script in playerScripts)
        {
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