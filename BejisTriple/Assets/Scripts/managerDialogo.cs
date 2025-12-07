using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class managerDialogo : MonoBehaviour
{
    // --- UI y diálogo (igual que antes) ---
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI dialogoTexto;
    public Image imagenPJ;
    public GameObject panelOpciones;

    [Header("Bloqueo de interacción")]
    public GameObject bloqueadorInput;

    [Header("Personaje con el que se habla")]
    public GameObject npcActual;

    public dialogoSO dialogoActual;
    private opcionDialogo opcionDialogo;
    public int indiceLineaActual = 0;
    public bool dialogoActivo = false;


    public AudioSource audioSource; 
    public AudioClip dialogo, boton;


    // --------- HINT 1 (igual que antes) ----------
    [Header("Hint 1 (cerca del NPC)")]
    public SpriteRenderer hintSprite1;
    public Transform player;
    public Transform detectionOrigin;
    public float detectionRadius1 = 5f;

    [Header("Animación Hint 1")]
    public float bounceInDuration = 0.5f;
    public float fadeOutDuration = 0.3f;
    public float bounceOvershoot = 1.2f;

    private bool isVisible1 = false;
    private bool isAnimating1 = false;
    private float animationTimer1 = 0f;
    private Vector3 originalScale1;
    private Color originalColor1;

    // --------- HINT 2 (fuera del radio del primero) ----------
    [Header("Hint 2 (cuando estás lejos)")]
    public SpriteRenderer hintSprite2;
    public float detectionRadius2 = 10f; // radio máximo para el segundo hint
    public float minScale2 = 0.2f;       // escala mínima cuando estás muy lejos
    public float maxScale2 = 1f;         // escala máxima cuando justo sales del radio del primero

    void Awake()
    {
        audioSource = panelDialogo.transform.parent.GetComponent<AudioSource>();
        if (detectionOrigin == null)
        {
            detectionOrigin = npcActual != null ? npcActual.transform : transform;
        }
    }

    void Start()
    {
        if (hintSprite1 != null)
        {
            originalScale1 = hintSprite1.transform.localScale;
            originalColor1 = hintSprite1.color;
            SetHintImmediate(hintSprite1, originalScale1, originalColor1, false);
        }

        if (hintSprite2 != null)
        {
            // arranca oculto
            SetHintImmediate(hintSprite2, hintSprite2.transform.localScale, hintSprite2.color, false);
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void Update()
    {
        if (dialogoActivo && Input.GetKeyDown(KeyCode.Space))
        {
            MuestraSiguienteLinea();
        }

        if (player == null || detectionOrigin == null) return;

        float distance = Vector2.Distance(detectionOrigin.position, player.position);

        // --- Lógica Hint 1 ---
        bool shouldShowHint1 = distance <= detectionRadius1;
        if (shouldShowHint1 && !isVisible1 && !isAnimating1)
            StartBounceIn();
        else if (!shouldShowHint1 && isVisible1 && !isAnimating1)
            StartFadeOut();

        if (isAnimating1)
        {
            animationTimer1 += Time.deltaTime;
            if (isVisible1) UpdateBounceIn();
            else UpdateFadeOut();
        }

        // --- Lógica Hint 2 ---
        if (hintSprite2 != null)
        {
            if (!shouldShowHint1) // solo aparece si el hint1 está oculto
            {
                float scaleFactor = Mathf.Clamp01(1f - (distance / detectionRadius2));
                float targetScale = Mathf.Lerp(minScale2, maxScale2, scaleFactor);

                hintSprite2.gameObject.SetActive(true);
                hintSprite2.transform.localScale = Vector3.one * targetScale;

                Color c = hintSprite2.color;
                c.a = 1f;
                hintSprite2.color = c;
            }
            else
            {
                hintSprite2.gameObject.SetActive(false);
            }
        }
    }

    // --------- DIALOGO (igual que antes) ----------
    public void IniciarDialogo(dialogoSO nuevoDialogo)
    {
        if (nuevoDialogo == null || nuevoDialogo.lineas.Length == 0) return;

        bloqueadorInput.SetActive(true);
        panelOpciones.SetActive(false);

        dialogoActual = nuevoDialogo;
        indiceLineaActual = 0;
        dialogoActivo = true;
        panelDialogo.SetActive(true);
        MostrarLinea(dialogoActual.lineas[indiceLineaActual]);

        SetHintImmediate(hintSprite1, originalScale1, originalColor1, false);
        if (hintSprite2 != null) hintSprite2.gameObject.SetActive(false);
    }

    void MuestraSiguienteLinea()
    {

        indiceLineaActual++;
        if (indiceLineaActual < dialogoActual.lineas.Length)
            MostrarLinea(dialogoActual.lineas[indiceLineaActual]);
        else
            TerminarDialogo();
    }

    void MostrarLinea(lineaDialogo linea)
    {
        if(!audioSource.isPlaying || audioSource.clip == dialogo)
        {
            audioSource.clip = dialogo;
            audioSource.Play();
        }
        
        nombreTexto.text = dialogoActual.nombrePJ;
        dialogoTexto.text = linea.textoDialogo;
        imagenPJ.sprite = linea.expresionPJ;
    }

    public void TerminarDialogo()
    {
        dialogoActivo = false;
        panelDialogo.SetActive(false);
        bloqueadorInput.SetActive(false);
        if(GameManager.Instance.ContadorDeMinijuegos() == npcActual.GetComponent<activarOpciones>().variableConOpciones)
        {
            npcActual.GetComponent<activarOpciones>().MostrarOpciones();
        }
    }

    // --------- ANIMACIONES HINT 1 ----------
    void StartBounceIn()
    {
        isAnimating1 = true;
        isVisible1 = true;
        animationTimer1 = 0f;
    }

    void UpdateBounceIn()
    {
        float progress = animationTimer1 / bounceInDuration;
        if (progress >= 1f)
        {
            hintSprite1.transform.localScale = originalScale1;
            Color c = hintSprite1.color; c.a = originalColor1.a;
            hintSprite1.color = c;
            isAnimating1 = false;
            return;
        }

        float scale = ElasticEaseOut(progress, bounceOvershoot);
        hintSprite1.transform.localScale = originalScale1 * scale;

        Color color = hintSprite1.color;
        color.a = originalColor1.a * progress;
        hintSprite1.color = color;
    }

    void StartFadeOut()
    {
        isAnimating1 = true;
        isVisible1 = false;
        animationTimer1 = 0f;
    }

    void UpdateFadeOut()
    {
        float progress = animationTimer1 / fadeOutDuration;
        if (progress >= 1f)
        {
            SetHintImmediate(hintSprite1, originalScale1, originalColor1, false);
            isAnimating1 = false;
            return;
        }

        float eased = 1f - (1f - progress) * (1f - progress);
        hintSprite1.transform.localScale = originalScale1 * (1f - eased);

        Color color = hintSprite1.color;
        color.a = originalColor1.a * (1f - progress);
        hintSprite1.color = color;
    }

    float ElasticEaseOut(float t, float overshoot)
    {
        if (t <= 0f) return 0f;
        if (t >= 1f) return 1f;
        float p = 0.3f;
        float s = p / 4f;
        return overshoot * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) + 1f;
    }

    void SetHintImmediate(SpriteRenderer sr, Vector3 origScale, Color origColor, bool visible)
    {
        if (sr == null) return;
        if (visible)
        {
            sr.transform.localScale = origScale;
            var c = origColor;
            c.a = origColor.a;
            sr.color = c;
            // NO desactivar el GameObject
        }
        else
        {
            sr.transform.localScale = Vector3.zero;
            var c = origColor;
            c.a = 0f;
            sr.color = c;
            // NO desactivar el GameObject
        }
    }
}
