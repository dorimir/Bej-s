using UnityEngine;
using TMPro;

public class Puntoserrores : MonoBehaviour
{
    public GameObject caidaObjetos; 
    public TextMeshProUGUI Puntos;
    //public Sprite spriteerrores;
    public int errorescometidos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void errores(int error){
        errorescometidos+=error;
        Puntos.text = "Puntos: " + errorescometidos;

    }

    // Update is called once per frame
    void Update()
    {
        if(errorescometidos>=3){
            caidaObjetos.GetComponent<caidaObjetos>().juegoacabado();
        }

    }
}
