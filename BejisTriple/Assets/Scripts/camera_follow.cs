using UnityEngine;


public class camera_follow : MonoBehaviour
{
    public Transform player;        

    public float height = 5f;      
    public float depth = -10f;

    // Límites de la cámara en el eje X
    public float xMin;
    public float xMax;
    private Vector3 offset;

    void Start(){
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calcula la posición deseada
        Vector3 newPos = new Vector3(
            player.position.x,  // seguimos la X del jugador
            height,              // altura fija
            depth                // profundidad fija
        );

        // Restringe la X
        newPos.x = Mathf.Clamp(newPos.x, xMin, xMax);

        // Aplica la posición
        transform.position = newPos;
    }
}
