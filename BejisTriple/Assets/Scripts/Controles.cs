using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 50f;
    private Rigidbody rb;
    private Animator animator;
    public bool miraizq =false;
    public bool espalda = false;

    AudioSource audioSource;
    public AudioClip walking, walkingGrass;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        procesarmovimiento();
    }

    void procesarmovimiento(){
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        if (hor != 0.0f || ver != 0.0f)
        {
            Vector3 dir = (transform.forward * ver + transform.right * hor).normalized;
            rb.position += dir * speed * Time.deltaTime;
            if(!audioSource.isPlaying)
            {
                if(SceneManager.GetActiveScene().name == "Rio")
                {
                    audioSource.clip = walkingGrass;
                }
                else audioSource.clip = walking;
                audioSource.Play();
            }
            
        }else audioSource.Stop();

        if(hor>0){
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsRunningL", false);
            animator.SetBool("quietoIzq", false);
            miraizq=false;
        }else if(hor==0 && !miraizq){
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
        else if(hor==0 && miraizq){
            animator.SetBool("quietoIzq",true);
            animator.SetBool("IsRunningL",false);
            animator.SetBool("IsRunning", false);
        }

        if(ver<0){
            if(miraizq){
                animator.SetBool("IsRunning",false);
                animator.SetBool("IsRunningL", true);
                animator.SetBool("quietoIzq",false);
                animator.SetBool("detras", false);
                miraizq=true;
                espalda=false;
            }
            else{
                animator.SetBool("IsRunningL", false);
                animator.SetBool("quietoIzq", false);
                animator.SetBool("IsRunning", true);
                animator.SetBool("detras", false);
                miraizq=false;
                espalda=false;
            }
        }else if(ver>0){
            if(miraizq){
                animator.SetBool("IsRunning",false);
                animator.SetBool("IsRunningL", true);
                animator.SetBool("quietoIzq",false);
                animator.SetBool("detras", true);
                miraizq=true;
                espalda=false;
            }else{
                animator.SetBool("IsRunningL", false);
                animator.SetBool("quietoIzq", false);
                animator.SetBool("IsRunning", true);
                animator.SetBool("detras", true);
                miraizq=false;
                espalda=true;
            }
        }
    }
}

