using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    public float speed = 50f;
    private Rigidbody2D rb;
    private Animator animator;
    public bool quietoIzq = false;
    public bool IsRunning=false;
    public bool IsRunningL=false;
    public bool miraizq=false;

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

        if(hor>0){
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsRunningL", false);
            animator.SetBool("quietoIzq", false);
            miraizq=false;
        }
        if(hor==0 && !miraizq){
            animator.SetBool("quietoIzq", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRunningL",false);
        }
        if(hor<0){
            animator.SetBool("IsRunning",false);
            animator.SetBool("IsRunningL", true);
            animator.SetBool("quietoIzq",false);
            miraizq=true;
        }
        if(hor==0 && miraizq){
            animator.SetBool("quietoIzq",true);
            animator.SetBool("IsRunningL",false);
            animator.SetBool("IsRunning", false);
        }
    }
}
