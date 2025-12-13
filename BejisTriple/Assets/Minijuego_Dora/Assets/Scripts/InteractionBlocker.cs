using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Bloqueador de interacciones - Colocar en un Canvas
/// Bloquea todos los clicks excepto el botón de pausa
/// </summary>
public class InteractionBlocker : MonoBehaviour
{
    public static InteractionBlocker Instance;

    [SerializeField] private GameObject blockerPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Crear el panel bloqueador si no existe
        if (blockerPanel == null)
        {
            CreateBlockerPanel();
        }

        // Empezar oculto
        HideBlocker();
    }

    private void CreateBlockerPanel()
    {
        // Crear GameObject para el panel
        GameObject panel = new GameObject("InteractionBlocker");
        panel.transform.SetParent(transform);

        // Configurar RectTransform para que ocupe toda la pantalla
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;

        // Añadir Image transparente (pero bloquea clicks)
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0); // Completamente transparente
        image.raycastTarget = true; // IMPORTANTE: bloquea los raycast

        blockerPanel = panel;
    }

    public void ShowBlocker()
    {
        if (blockerPanel != null)
        {
            blockerPanel.SetActive(true);
        }
    }

    public void HideBlocker()
    {
        if (blockerPanel != null)
        {
            blockerPanel.SetActive(false);
        }
    }
}