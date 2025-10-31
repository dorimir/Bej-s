using UnityEngine;

public class Anzuelo : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    public float speed = 1;
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
    }
}
