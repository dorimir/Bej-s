using UnityEngine;

/// <summary>
/// Sistema de cuerdas del arco - COLOCAR EN EL ARCO, NO EN LA FLECHA
/// Las cuerdas siguen automáticamente a la flecha activa
/// </summary>
public class BowString : MonoBehaviour
{
    [Header("Puntos del Arco")]
    [SerializeField] private Transform topStringPoint;    // Punto superior del arco
    [SerializeField] private Transform bottomStringPoint; // Punto inferior del arco
    [SerializeField] private Transform restPosition;      // Posición de reposo (centro)

    [Header("Configuración Visual")]
    [SerializeField] private float stringWidth = 0.05f;
    [SerializeField] private Color stringColor = new Color(0.8f, 0.7f, 0.5f); // Color cuerda
    [SerializeField] private int sortingOrder = -1; // ✨ Ajustable desde Inspector
    [Tooltip("Dejar vacío para crear automáticamente")]
    [SerializeField] private LineRenderer topStringLine;
    [Tooltip("Dejar vacío para crear automáticamente")]
    [SerializeField] private LineRenderer bottomStringLine;

    private Transform currentArrow; // Flecha activa actual
    private bool isDrawn = false;

    private void Start()
    {
        SetupLineRenderers();

        // ✨ Empezar con las cuerdas ocultas
        SetStringsVisible(false);
    }

    private void SetupLineRenderers()
    {
        // Crear cuerda superior si no existe
        if (topStringLine == null)
        {
            GameObject topString = new GameObject("TopString");
            topString.transform.SetParent(transform);
            topStringLine = topString.AddComponent<LineRenderer>();
        }
        ConfigureLineRenderer(topStringLine);

        // Crear cuerda inferior si no existe
        if (bottomStringLine == null)
        {
            GameObject bottomString = new GameObject("BottomString");
            bottomString.transform.SetParent(transform);
            bottomStringLine = bottomString.AddComponent<LineRenderer>();
        }
        ConfigureLineRenderer(bottomStringLine);
    }

    private void ConfigureLineRenderer(LineRenderer line)
    {
        line.positionCount = 2;
        line.startWidth = stringWidth;
        line.endWidth = stringWidth;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = stringColor;
        line.endColor = stringColor;
        line.useWorldSpace = true;

        // ✨ Usar el valor configurable desde Inspector
        line.sortingLayerName = "Default";
        line.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        UpdateStringPositions();
    }

    private void UpdateStringPositions()
    {
        if (topStringPoint == null || bottomStringPoint == null) return;

        Vector3 targetPoint;

        if (isDrawn && currentArrow != null)
        {
            // Las cuerdas van a la flecha mientras se arrastra
            targetPoint = currentArrow.position;
        }
        else
        {
            // En reposo, las cuerdas van al punto central
            targetPoint = restPosition != null ? restPosition.position :
                         (topStringPoint.position + bottomStringPoint.position) / 2f;
        }

        // Mantener en el mismo plano Z
        targetPoint.z = topStringPoint.position.z;

        // Actualizar cuerdas
        if (topStringLine != null)
        {
            topStringLine.SetPosition(0, topStringPoint.position);
            topStringLine.SetPosition(1, targetPoint);
        }

        if (bottomStringLine != null)
        {
            bottomStringLine.SetPosition(0, bottomStringPoint.position);
            bottomStringLine.SetPosition(1, targetPoint);
        }
    }

    /// <summary>
    /// Llamar cuando se suelte la flecha
    /// </summary>
    public void OnBowReleased()
    {
        isDrawn = false;
        currentArrow = null;

        // ✨ Ocultar las cuerdas al soltar
        SetStringsVisible(false);
    }

    /// <summary>
    /// Llamar cuando se empiece a arrastrar la flecha
    /// </summary>
    public void OnBowDrawn(Transform arrow)
    {
        currentArrow = arrow;
        isDrawn = true;

        // ✨ Mostrar las cuerdas al arrastrar
        SetStringsVisible(true);
    }

    public void SetStringsVisible(bool visible)
    {
        if (topStringLine != null) topStringLine.enabled = visible;
        if (bottomStringLine != null) bottomStringLine.enabled = visible;
    }
}