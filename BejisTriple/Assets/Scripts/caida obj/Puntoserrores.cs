using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class Puntoserrores : MonoBehaviour
{
    public GameObject caidaObjetos; 
    public TextMeshProUGUI Puntos;
    private Animator animator;
    //public Sprite spriteerrores;
    public int errorescometidos=0;
    public int putosobt;
    public GameObject e1;
    public GameObject e2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void errores(int error){
        
        errorescometidos+=error;

        if(errorescometidos==1){
            e1.SetActive(true); 
        }
        else if(errorescometidos==2){
            e1.SetActive(false); 
            e2.SetActive(true); 

        }  
        

    }

    public void ptos(int puntos){
        putosobt+=puntos;
        Puntos.text = "Puntos: " + putosobt;
    }

    // Update is called once per frame
    void Update()
    {
        if(errorescometidos>=3){
            caidaObjetos.GetComponent<caidaObjetos>().juegoacabado();
        }

    }
}
