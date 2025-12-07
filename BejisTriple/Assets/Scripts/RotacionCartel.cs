using UnityEngine;

public class RotacionCartel : MonoBehaviour
{
    public float ampZ = 5f;     // amplitud en grados (pequeña)
    public float speed = 2f;    // velocidad de oscilación

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * ampZ;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

