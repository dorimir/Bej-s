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
    public int errorescometidos;
    public GameObject e1;
    public GameObject e2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void errores(int error){
        errorescometidos+=error;
        Puntos.text = "Puntos: " + errorescometidos;
        if(errorescometidos==1) e1.SetActive(true);
        else if(errorescometidos==2) e2.SetActive(true); 
        

    }

    // Update is called once per frame
    void Update()
    {
        if(errorescometidos>=3){
            caidaObjetos.GetComponent<caidaObjetos>().juegoacabado();
        }

    }
}
