using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Transform player;        // Arrastra aquí el personaje en el Inspector
    public float height = 5f;       // Altura de la cámara respecto al jugador
    public float depth = -10f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector3 newPos = new Vector3(
            player.position.x,      // sólo seguimos la X del jugador
            height,                 // altura fija
            depth  // Z del jugador + separación (o puedes fijar un valor)
        );
        transform.position = newPos;
    }
}
