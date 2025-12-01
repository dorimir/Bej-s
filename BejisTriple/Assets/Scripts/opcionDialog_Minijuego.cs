using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class opcionDialog_Minijuego : opcionDialogo
{
    //Este es el archivo de cada boton que inicia un minijuego
    public string Minijuego;


    public override void activarDIalogo()
    {
        
        managerDialogo manager = FindObjectOfType<managerDialogo>();
        if (manager != null)
        {
            Debug.Log("El manager dialogo no es null");
            SceneManager.LoadScene(Minijuego, LoadSceneMode.Single);
        } else Debug.Log("El manager dialogo es null, no se puede abrir");
    }
}
