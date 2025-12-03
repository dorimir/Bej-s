using UnityEngine;

public class Trayectoria : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform puntoInicial; // Tu flecha
    [SerializeField] private Transform shootPosition; // El punto de anclaje
    [SerializeField] private GameObject puntoPrefab;

    [Header("Configuración")]
    [SerializeField] private int numeroDePuntos = 30;
    [SerializeField] private float tiempoEntreSimulacion = 0.05f;
    [SerializeField] private float launchForce = 15f; // Debe coincidir con Bird3D

    [Header("Visual")]
    [SerializeField] private float opacidad = 0.5f;

    private GameObject[] puntos;
    private Camera mainCam;
    private bool isPressed = false;

    void Start()
    {
        mainCam = Camera.main;

        // Crear los puntos de la trayectoria
        puntos = new GameObject[numeroDePuntos];

        for (int i = 0; i < numeroDePuntos; i++)
        {
            puntos[i] = Instantiate(puntoPrefab, transform);
            puntos[i].SetActive(false);

            // Aplicar opacidad
            SpriteRenderer sr = puntos[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = opacidad;
                sr.color = color;
            }
        }
    }

    void Update()
    {
        if (isPressed && puntoInicial != null && shootPosition != null)
        {
            MostrarTrayectoria();
        }
        else
        {
            OcultarPuntos();
        }
    }

    // Métodos públicos para que Bird3D los llame
    public void OnArrowPressed()
    {
        Debug.Log("Trayectoria activada");
        isPressed = true;
    }

    public void OnArrowReleased()
    {
        isPressed = false;
        OcultarPuntos();
    }

    void MostrarTrayectoria()
    {
        Vector3 birdPos = puntoInicial.position;
        Vector3 shootPos = shootPosition.position;

        // Calcular la dirección de lanzamiento (igual que en Bird3D)
        Vector3 launchDirection = shootPos - birdPos;
        launchDirection.z = 90;

        // Calcular la velocidad inicial
        float stretchDistance = launchDirection.magnitude;
        Vector3 velocidad = launchDirection.normalized * launchForce * stretchDistance;

        for (int i = 0; i < numeroDePuntos; i++)
        {
            // Calcular tiempo de simulación
            float tiempo = i * tiempoEntreSimulacion;

            // Calcular posición usando física (igual que DrawTrajectory en Bird3D)
            Vector3 posicion = birdPos + velocidad * tiempo + 0.5f * Physics.gravity * tiempo * tiempo;
            posicion.z = birdPos.z;

            puntos[i].transform.position = posicion;
            puntos[i].SetActive(true);

            // Aplicar opacidad actualizada
            SpriteRenderer sr = puntos[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = opacidad;
                sr.color = color;
            }
        }
    }

    // Para cambiar la opacidad en tiempo real
    public void CambiarOpacidad(float nuevaOpacidad)
    {
        opacidad = Mathf.Clamp01(nuevaOpacidad);
    }

    // Para activar/desactivar la trayectoria
    public void MostrarLinea(bool mostrar)
    {
        isPressed = mostrar;

        if (puntos != null && !mostrar)
        {
            OcultarPuntos();
        }
    }

    private void OcultarPuntos()
    {
        if (puntos != null)
        {
            foreach (GameObject punto in puntos)
            {
                punto.SetActive(false);
            }
        }
    }

    // Para ajustar la fuerza en tiempo real
    public void CambiarFuerza(float nuevaFuerza)
    {
        launchForce = nuevaFuerza;
    }
}