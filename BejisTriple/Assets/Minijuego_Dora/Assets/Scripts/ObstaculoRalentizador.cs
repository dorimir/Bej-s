using UnityEngine;
using System.Collections;

public class ObstaculoRalentizador : MonoBehaviour
{
    [Header("Configuración de Ralentización")]
    [Range(0f, 1f)]
    [Tooltip("Porcentaje de velocidad que mantiene el proyectil (0 = detiene completamente, 1 = sin ralentización)")]
    public float factorRalentizacion = 0.5f;

    [Header("Referencia al Proyectil")]
    [Tooltip("Arrastra aquí tu GameObject del proyectil/player")]
    public GameObject proyectil;

    [Header("Configuración de Física")]
    [Tooltip("Congelar la coordenada Z para mantener el movimiento 2D")]
    public bool congelarEjeZ = true;

    [Header("Animación de Impacto")]
    [Tooltip("Escala máxima al ser golpeado")]
    public float escalaImpacto = 1.3f;
    [Tooltip("Duración de la animación de escala (segundos)")]
    public float duracionAnimacionEscala = 0.2f;
    [Tooltip("Duración del fade out (segundos)")]
    public float duracionFadeOut = 1.0f;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;
    private bool fueGolpeado = false;

    void Start()
    {
        ConfigurarComponentes();
        escalaOriginal = transform.localScale;
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
        if (proyectil != null && other.gameObject == proyectil && !fueGolpeado)
        {
            fueGolpeado = true;
            RalentizarProyectil(other);
            StartCoroutine(AnimarImpactoYDesaparecer());
        }
    }

    private void RalentizarProyectil(Collider proyectilCollider)
    {
        Rigidbody proyectilRb = proyectilCollider.GetComponent<Rigidbody>();

        if (proyectilRb != null)
        {
            proyectilRb.linearVelocity *= factorRalentizacion;
            proyectilRb.angularVelocity *= factorRalentizacion;

            Debug.Log($"Proyectil ralentizado al {factorRalentizacion * 100}%");
        }
    }

    private IEnumerator AnimarImpactoYDesaparecer()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionAnimacionEscala)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracionAnimacionEscala;

            float escala = Mathf.Lerp(1f, escalaImpacto, Mathf.Sin(progreso * Mathf.PI));
            transform.localScale = escalaOriginal * escala;

            yield return null;
        }

        transform.localScale = escalaOriginal;

        if (spriteRenderer != null)
        {
            Color colorOriginal = spriteRenderer.color;
            tiempoTranscurrido = 0f;

            while (tiempoTranscurrido < duracionFadeOut)
            {
                tiempoTranscurrido += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / duracionFadeOut);

                spriteRenderer.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, alpha);

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