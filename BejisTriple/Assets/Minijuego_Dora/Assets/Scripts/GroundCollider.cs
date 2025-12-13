using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string groundTag = "suelo";

    // Referencia al SoundController
    private SoundController soundController;

    private void Start()
    {
        // Buscar el SoundController en la escena
        soundController = FindObjectOfType<SoundController>();
        if (soundController == null) ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Solo reproducir sonido si colisiona jugador con suelo
        if (collision.gameObject.CompareTag(playerTag) && this.CompareTag(groundTag))
        {
            // Reproducir sonido de colisión con suelo

            soundController?.PlayCollisionGround();
        }
    }
}
