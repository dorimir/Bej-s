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

    private bool isPressed = false;
    private Camera mainCam;
    private SpringJoint springJoint;
    private bool hasLaunched = false;
    private bool isPersistent = false; // NUEVO: indica si la flecha es persistente

    private void Start()
    {
        birdRigidbody = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        springJoint = GetComponent<SpringJoint>();

        birdRigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY;

        birdRigidbody.centerOfMass = centerOfMassOffset;
    }

    private void Update()
    {
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
            transform.position = newPos;

            Vector3 launchDirection = shootPos - newPos;
            if (launchDirection.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (trajectoryLine != null)
            {
                DrawTrajectory(-direction * launchForce);
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

        // SOLO destruir si NO es persistente
        if (!isPersistent)
        {
            Destroy(gameObject, 20f);
        }
        else
        {
            Debug.Log("[Bird3D] Flecha marcada como persistente, NO se destruirá");
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
        // Marcar como persistente cuando colisiona
        if (!isPersistent)
        {
            isPersistent = true;
            Debug.Log("[Bird3D] Flecha colisionó, marcada como persistente");

            // Notificar al ContadorDistancia
            ContadorDistancia contador = FindFirstObjectByType<ContadorDistancia>();
            if (contador != null)
            {
                contador.OnArrowCollided();
            }
        }

        // Si toca el objeto objetivo, cambiar de escena
        if (targetObject != null && collision.gameObject == targetObject)
        {
            SceneManager.LoadScene("TiroConArco");
        }
    }

    private void DrawTrajectory(Vector3 velocity)
    {
        if (trajectoryLine == null) return;

        trajectoryLine.enabled = true;
        int segments = 20;
        Vector3[] points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float t = i * 0.1f;
            points[i] = birdRigidbody.position + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i].z = birdRigidbody.position.z;
        }

        trajectoryLine.positionCount = segments;
        trajectoryLine.SetPositions(points);
    }

    // Método público para marcar como persistente desde otros scripts
    public void MarkAsPersistent()
    {
        isPersistent = true;
        Debug.Log("[Bird3D] Flecha marcada manualmente como persistente");
    }
}