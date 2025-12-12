using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird3D : MonoBehaviour
{
    [SerializeField] private Rigidbody birdRigidbody;
    [SerializeField] public Rigidbody shootRigidbody;
    [SerializeField] public GameObject birdPrefab;
    [SerializeField] public Transform birdSpawnPos;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float launchForce = 15f;
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0.5f, 0, 0);
    [SerializeField] private Trayectoria trayectoria;
    [SerializeField] private Transform bowTransform;
    [SerializeField] private BowString bowString;

    private bool isPressed = false;
    private Camera mainCam;
    private SpringJoint springJoint;
    private bool hasLaunched = false;
    private bool isPersistent = false;

    // Límites de ángulo para arrastre: de 90° a 270° (mitad izquierda)
    private float minDragAngle = 90f;   // Arriba
    private float maxDragAngle = 270f;  // Abajo

    private void Start()
    {
        birdRigidbody = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        springJoint = GetComponent<SpringJoint>();

        birdRigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY;

        birdRigidbody.centerOfMass = Vector3.zero;
    }

    private void Update()
    {
        // Forzar rotación solo en Z (2D)
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, currentRotation.z);

        if (isPressed)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCam.WorldToScreenPoint(birdRigidbody.position).z;
            mousePos = mainCam.ScreenToWorldPoint(mousePos);

            Vector3 shootPos = shootRigidbody.position;
            Vector3 direction = mousePos - shootPos;
            direction.z = 0;

            if (direction.magnitude > maxDistance)
            {
                direction = direction.normalized * maxDistance;
            }

            Vector3 newPos = shootPos + direction;
            newPos.z = shootPos.z;

            Vector3 dragDirection = newPos - shootPos;

            if (dragDirection.magnitude > 0.1f)
            {
                // Calcular ángulo del arrastre
                float dragAngle = Mathf.Atan2(dragDirection.y, dragDirection.x) * Mathf.Rad2Deg;

                // Normalizar el ángulo a 0-360
                if (dragAngle < 0) dragAngle += 360f;

                // ✨ NUEVA LÓGICA: Restricción suave sin saltos
                float clampedAngle = dragAngle;

                // Si está en la zona prohibida (0-90 o 270-360)
                if (dragAngle < 90f || dragAngle > 270f)
                {
                    // Calcular distancia a cada límite
                    float distanceTo90 = dragAngle < 90f ? 90f - dragAngle : 360f - dragAngle + 90f;
                    float distanceTo270 = dragAngle > 270f ? dragAngle - 270f : 270f - dragAngle;

                    // Quedarse en el límite más cercano
                    clampedAngle = (distanceTo90 < distanceTo270) ? 90f : 270f;
                }

                // Usar el ángulo restringido
                float dragAngleRad = clampedAngle * Mathf.Deg2Rad;
                Vector3 restrictedDragDirection = new Vector3(Mathf.Cos(dragAngleRad), Mathf.Sin(dragAngleRad), 0);

                float distance = Mathf.Min(dragDirection.magnitude, maxDistance);
                newPos = shootPos + restrictedDragDirection * distance;
                newPos.z = shootPos.z;

                transform.position = newPos;

                // La flecha apunta en la dirección OPUESTA (hacia donde se lanzará)
                Vector3 launchDirection = shootPos - newPos;
                float launchAngle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0, 0, launchAngle);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // Rotar el arco también
                if (bowTransform != null)
                {
                    bowTransform.rotation = Quaternion.Euler(0, 0, launchAngle);
                }

                // Mostrar trayectoria en la dirección de lanzamiento
                if (trajectoryLine != null)
                {
                    DrawTrajectory(launchDirection.normalized * launchForce * distance);
                }
            }
        }

        if (hasLaunched && !isPersistent && birdRigidbody.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(birdRigidbody.linearVelocity.y, birdRigidbody.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
        birdRigidbody.isKinematic = true;

        // 🔊 Sonido de apuntar
        SoundController.Instance?.PlayApuntar();


        if (bowString != null)
        {
            bowString.OnBowDrawn(transform);
        }

        birdRigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        birdRigidbody.isKinematic = false;
        hasLaunched = true;

        SoundController.Instance.PlayArrowShot();
        if (trayectoria != null)
        {
            trayectoria.OnArrowReleased();
        }

        if (bowString != null)
        {
            bowString.OnBowReleased();
        }

        birdRigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY;

        Vector3 shootPos = shootRigidbody.position;
        Vector3 birdPos = birdRigidbody.position;
        Vector3 launchDirection = shootPos - birdPos;
        launchDirection.z = 0;

        birdRigidbody.linearVelocity = Vector3.zero;
        birdRigidbody.angularVelocity = Vector3.zero;

        float stretchDistance = launchDirection.magnitude;
        birdRigidbody.linearVelocity = launchDirection.normalized * launchForce * stretchDistance;

        float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }

        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(0.05f);

        if (springJoint != null)
        {
            Destroy(springJoint);
        }

        this.enabled = false;

        if (!isPersistent)
        {
            Destroy(gameObject, 20f);
        }

        yield return new WaitForSeconds(5f);

        if (targetObject == null && !isPersistent)
        {
            if (birdPrefab != null && birdSpawnPos != null)
            {
                Instantiate(birdPrefab, birdSpawnPos.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPersistent)
        {
            isPersistent = true;
            Debug.Log("[Bird3D] Flecha colisionó, marcada como persistente");

            ContadorDistancia contador = FindFirstObjectByType<ContadorDistancia>();
            if (contador != null)
            {
                contador.OnArrowCollided();
            }
        }

        // 🔊 Sonidos según lo que golpea
        if (collision.collider.CompareTag("Ground"))
            SoundController.Instance?.PlayCollisionGround();

        else if (collision.collider.CompareTag("Obstaculo"))
            SoundController.Instance?.PlayCollisionObstaculo();

        else if (collision.collider.CompareTag("Potenciador"))
            SoundController.Instance?.PlayCollisionPotenciador();

        if (targetObject != null && collision.gameObject == targetObject)
        {
            SceneManager.LoadScene("TiroConArco");
        }
    }

    private void DrawTrajectory(Vector3 velocity)
    {
        if (trajectoryLine == null) return;

        int segments = 30;                // Más puntos = línea más suave
        float timeStep = 0.05f;           // Intervalo de tiempo entre puntos

        Vector3[] points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float t = i * timeStep;
            // Fórmula física: s = v*t + 0.5*g*t^2
            points[i] = birdRigidbody.position + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i].z = birdRigidbody.position.z;  // Mantener en el plano 2.5D
        }

        trajectoryLine.positionCount = segments;
        trajectoryLine.SetPositions(points);
        trajectoryLine.enabled = true;
    }

    public void MarkAsPersistent()
    {
        isPersistent = true;
        Debug.Log("[Bird3D] Flecha marcada manualmente como persistente");
    }
}