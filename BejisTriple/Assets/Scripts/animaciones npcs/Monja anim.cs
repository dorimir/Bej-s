using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monjaanim : MonoBehaviour, IInteractable
{

    private Animator animator;
    public void Interact(){
        animator.SetBool("idle", false);
        
    }
        
}

