using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trayectoria : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform puntoInicial; // Tu flecha
    [SerializeField] private Transform shootPosition; // El punto de anclaje

    [Header("Configuración")]
    [SerializeField] private int numeroDePuntos = 30;
    [SerializeField] private float tiempoEntreSimulacion = 0.05f;
    [SerializeField] private float launchForce = 15f; // Debe coincidir con Bird3D

    [Header("Visual")]
    [SerializeField] private float grosor = 0.05f;

    private LineRenderer lineRenderer;
    private bool isPressed = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = numeroDePuntos;
            lineRenderer.enabled = false;
            lineRenderer.startWidth = grosor;
            lineRenderer.endWidth = grosor;
        }
        else
        {
            Debug.LogError("[Trayectoria] No se encontró LineRenderer!");
        }
    }

    void Update()
    {
        if (isPressed && puntoInicial != null && shootPosition != null)
        {
            DibujarTrayectoria();
        }
        else
        {
            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }
    }

    // Métodos públicos para Bird3D
    public void OnArrowPressed()
    {
        isPressed = true;
    }

    public void OnArrowReleased()
    {
        isPressed = false;
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void DibujarTrayectoria()
    {
        if (lineRenderer == null) return;

        Vector3 birdPos = puntoInicial.position;
        Vector3 shootPos = shootPosition.position;

        // Dirección y velocidad de lanzamiento
        Vector3 launchDir = shootPos - birdPos;
        launchDir.z = 0;
        float stretchDistance = launchDir.magnitude;
        Vector3 velocity = launchDir.normalized * launchForce * stretchDistance;

        // Calcular puntos de la línea
        for (int i = 0; i < numeroDePuntos; i++)
        {
            float t = i * tiempoEntreSimulacion;
            Vector3 pointPos = birdPos + velocity * t + 0.5f * Physics.gravity * t * t;
            pointPos.z = birdPos.z; // Mantener en 2.5D
            lineRenderer.SetPosition(i, pointPos);
        }

        lineRenderer.enabled = true;
    }

    // Para cambiar fuerza o número de puntos en tiempo real
    public void CambiarFuerza(float nuevaFuerza)
    {
        launchForce = nuevaFuerza;
    }

    public void CambiarNumeroDePuntos(int nuevosPuntos)
    {
        numeroDePuntos = nuevosPuntos;
        if (lineRenderer != null)
            lineRenderer.positionCount = numeroDePuntos;
    }
}
