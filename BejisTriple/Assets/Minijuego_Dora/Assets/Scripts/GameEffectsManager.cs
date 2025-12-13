using System.Collections;
using UnityEngine;

public class GameEffectsManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform bowTransform;
    [SerializeField] private Transform arrowTransform;

    [Header("Confetti Victoria")]
    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private int confettiCount = 30;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float confettiLifetime = 3f;
    [SerializeField]
    private Color[] confettiColors = new Color[]
    {
        Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.cyan
    };

    [Header("Zoom Derrota")]
    [SerializeField] private float zoomDuration = 2f;
    [SerializeField] private float zoomDistance = 5f;

    [Header("Bouncy Animation")]
    [SerializeField] private float bounceDuration = 0.8f;

    private Vector3 bowOriginalScale;
    private Vector3 arrowOriginalScale;
    private bool isZooming = false;

    private void Start()
    {
        // Auto-buscar cámara si no está asignada
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (bowTransform != null)
            bowOriginalScale = bowTransform.localScale;
        if (arrowTransform != null)
            arrowOriginalScale = arrowTransform.localScale;

        // Solo reproducir bouncy si hay referencias
        if (bowTransform != null || arrowTransform != null)
        {
            PlayBouncyAnimation();
        }
    }

    // ===== BOUNCY ANIMATION =====
    public void PlayBouncyAnimation()
    {
        StartCoroutine(BouncyAnimationCoroutine());
    }

    private IEnumerator BouncyAnimationCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / bounceDuration;

            float bounceValue = Mathf.Sin(progress * Mathf.PI * 2f) * (1f - progress);
            float scale = 1f + (bounceValue * 0.3f);

            if (bowTransform != null)
                bowTransform.localScale = bowOriginalScale * scale;

            if (arrowTransform != null)
                arrowTransform.localScale = arrowOriginalScale * scale;

            yield return null;
        }

        if (bowTransform != null)
            bowTransform.localScale = bowOriginalScale;
        if (arrowTransform != null)
            arrowTransform.localScale = arrowOriginalScale;
    }

    // ===== CONFETTI VICTORIA =====
    public void PlayConfetti(Vector3 position)
    {
        StartCoroutine(SpawnConfetti(position));
    }

    private IEnumerator SpawnConfetti(Vector3 position)
    {

        for (int i = 0; i < confettiCount; i++)
        {
            GameObject confettiPiece;

            if (confettiPrefab != null)
            {
                confettiPiece = Instantiate(confettiPrefab, position, Random.rotation);
            }
            else
            {
                // Crear diferentes formas aleatorias
                PrimitiveType[] shapes = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Cylinder };
                confettiPiece = GameObject.CreatePrimitive(shapes[Random.Range(0, shapes.Length)]);
                confettiPiece.transform.position = position;

                // Tamaños y formas variadas para más efecto
                if (confettiPiece.GetComponent<MeshFilter>().sharedMesh.name.Contains("Cube"))
                {
                    confettiPiece.transform.localScale = new Vector3(
                        Random.Range(0.1f, 0.3f),
                        Random.Range(0.05f, 0.1f),
                        Random.Range(0.1f, 0.3f)
                    );
                }
                else
                {
                    confettiPiece.transform.localScale = Vector3.one * Random.Range(0.1f, 0.25f);
                }

                Destroy(confettiPiece.GetComponent<Collider>());
            }

            // Material con colores brillantes
            Renderer renderer = confettiPiece.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = confettiColors[Random.Range(0, confettiColors.Length)];
                mat.SetFloat("_Metallic", 0.5f);
                mat.SetFloat("_Glossiness", 0.8f);
                renderer.material = mat;
            }

            Rigidbody rb = confettiPiece.GetComponent<Rigidbody>();
            if (rb == null)
                rb = confettiPiece.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.mass = 0.1f;

            // Explosión más espectacular
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y); // Siempre hacia arriba
            rb.linearVelocity = randomDirection * explosionForce + Vector3.up * explosionForce * 0.5f;
            rb.angularVelocity = Random.insideUnitSphere * 15f;

            Destroy(confettiPiece, confettiLifetime);

            if (i % 3 == 0)
                yield return new WaitForSeconds(0.02f);
        }
    }

    // ===== ZOOM DERROTA =====
    public void ZoomToArrow(Transform target)
    {

        if (!isZooming && target != null && mainCamera != null)
        {
            StartCoroutine(ZoomCoroutine(target));
        }
        else
        {
 
        }
    }

    private IEnumerator ZoomCoroutine(Transform target)
    {
        isZooming = true;

        Vector3 originalPosition = mainCamera.transform.position;
        Quaternion originalRotation = mainCamera.transform.rotation;

        Vector3 directionToTarget = (target.position - mainCamera.transform.position).normalized;
        Vector3 targetPosition = target.position - directionToTarget * zoomDistance;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

        float elapsedTime = 0f;

        // Zoom ina
        while (elapsedTime < zoomDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (zoomDuration / 2f);

            mainCamera.transform.position = Vector3.Lerp(originalPosition, targetPosition, progress);
            mainCamera.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, progress);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;

        // Zoom out
        while (elapsedTime < zoomDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (zoomDuration / 2f);

            mainCamera.transform.position = Vector3.Lerp(targetPosition, originalPosition, progress);
            mainCamera.transform.rotation = Quaternion.Lerp(targetRotation, originalRotation, progress);

            yield return null;
        }

        mainCamera.transform.position = originalPosition;
        mainCamera.transform.rotation = originalRotation;

        isZooming = false;
    }
}