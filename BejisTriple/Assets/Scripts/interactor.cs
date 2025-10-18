using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable{
    public void Interact();
}

public class interactor : MonoBehaviour
{
    
    public Transform InteractorSource;
    public float InteractRange = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) ){
            Collider[] hits = Physics.OverlapSphere(InteractorSource.position, InteractRange);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                    // Opcional: salir del loop si solo quieres interactuar con el primero
                    break;
                }
            }
        }
    }
}