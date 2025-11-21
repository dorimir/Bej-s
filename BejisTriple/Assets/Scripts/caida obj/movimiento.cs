using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    public float speed = 50f;
    private Rigidbody2D rb;
    private Animator animator;
    public bool miraizq = false;
    public bool espalda = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movimiento2d();
    }

    void movimiento2d()
    {
        float hor = Input.GetAxisRaw("Horizontal");

        if (hor != 0.0f)
        {
            Vector2 dir = (transform.right * hor).normalized;
            rb.position += dir * speed * Time.deltaTime;
        }
    }
}
