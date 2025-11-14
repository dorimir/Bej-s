using UnityEngine;

public class efectocaida : MonoBehaviour
{
    Vector3 startPos;
    public GameObject obj;

    public float ampX = 0.75f;   // amplitud pequeña a izquierda/derecha

    public float speedX = 2.0f;  // velocidad del movimiento

    void Start()
    {
        startPos = obj.transform.position;
    }

    void Update()
    {
        float t = Time.time * speedX;
        float x = Mathf.Sin(t) * ampX;

        transform.position = new Vector3(startPos.x + x, obj.transform.position.y, startPos.z);
    }
}
