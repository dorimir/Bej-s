using UnityEngine;
using System.Collections;

public class Potenciador : MonoBehaviour
{
    [Header("Configuración de Impulso")]
    [Range(1f, 3f)]
    [Tooltip("Multiplicador de velocidad (1 = sin cambio, 2 = doble velocidad)")]
    public float factorImpulso = 1.5f;

    [Header("Impulso Vertical")]
    [Tooltip("Fuerza adicional hacia arriba")]
    public float fuerzaArriba = 5f;

    [Header("Referencia al Proyectil")]
    [Tooltip("Arrastra aquí tu GameObject del proyectil/player")]
    public GameObject proyectil;

    [Header("Configuración de Física")]
    [Tooltip("Congelar la coordenada Z para mantener el movimiento 2D")]
    public bool congelarEjeZ = true;

    [Header("Animación de Activación")]
    [Tooltip("Escala máxima al ser activado")]
    public float escalaActivacion = 1.3f;
    [Tooltip("Duración de la animación de escala (segundos)")]
    public float duracionAnimacionEscala = 0.2f;
    [Tooltip("Duración del fade out (segundos)")]
    public float duracionFadeOut = 1.0f;
    [Tooltip("Color final al desaparecer (amarillo por defecto)")]
    public Color colorFinal = Color.yellow;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;
    private Color colorOriginal;
    private bool fueActivado = false;

    void Start()
    {
        ConfigurarComponentes();
        escalaOriginal = transform.localScale;

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    private void ConfigurarComponentes()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;

            if (congelarEjeZ)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ |
                               RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el proyectil por nombre o por referencia
        bool esProyectil = (proyectil != null && other.gameObject == proyectil) ||
                          (proyectil != null && other.gameObject.name == proyectil.name);

        if (esProyectil && !fueActivado)
        {
            fueActivado = true;
            PotenciarProyectil(other);
            StartCoroutine(AnimarActivacionYDesaparecer());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verificar si es el proyectil por nombre o por referencia
        bool esProyectil = (proyectil != null && other.gameObject == proyectil) ||
                          (proyectil != null && other.gameObject.name == proyectil.name);

        if (esProyectil && !fueActivado)
        {
            fueActivado = true;
            PotenciarProyectil(other);
            StartCoroutine(AnimarActivacionYDesaparecer());
        }
    }

    private void PotenciarProyectil(Collider proyectilCollider)
    {
        Rigidbody proyectilRb = proyectilCollider.GetComponent<Rigidbody>();

        if (proyectilRb != null)
        {
            // Guardar la dirección actual
            Vector3 velocidadAntes = proyectilRb.linearVelocity;

            // Multiplicar la velocidad actual (impulso adelante)
            Vector3 nuevaVelocidad = velocidadAntes * factorImpulso;

            // Añadir impulso hacia arriba
            nuevaVelocidad += new Vector3(0, fuerzaArriba, 0);

            // Asignar la nueva velocidad
            proyectilRb.linearVelocity = nuevaVelocidad;

            // También potenciar la velocidad angular
            proyectilRb.angularVelocity *= factorImpulso;
        }
    }

    private IEnumerator AnimarActivacionYDesaparecer()
    {
        // Fase 1: Animación de escala (activación)
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionAnimacionEscala)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracionAnimacionEscala;

            float escala = Mathf.Lerp(1f, escalaActivacion, Mathf.Sin(progreso * Mathf.PI));
            transform.localScale = escalaOriginal * escala;

            yield return null;
        }

        transform.localScale = escalaOriginal;

        // Fase 2: Fade out con cambio de color a amarillo
        if (spriteRenderer != null)
        {
            tiempoTranscurrido = 0f;

            while (tiempoTranscurrido < duracionFadeOut)
            {
                tiempoTranscurrido += Time.deltaTime;
                float progreso = tiempoTranscurrido / duracionFadeOut;

                // Interpolar color de original a amarillo (más agresivo)
                Color colorActual = Color.Lerp(colorOriginal, colorFinal, progreso);

                // Interpolar alpha de 1 a 0
                float alpha = Mathf.Lerp(1f, 0f, progreso);

                spriteRenderer.color = new Color(colorActual.r, colorActual.g, colorActual.b, alpha);

                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(duracionFadeOut);
        }

        Destroy(gameObject);
    }
}