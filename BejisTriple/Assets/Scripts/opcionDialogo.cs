using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class opcionDialogo : MonoBehaviour
{
    //Este es el archivo de cada boton
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public GameObject panelOpciones;

    public dialogoSO dialogo;

    public void ExitDialogo()
    {
        panelDialogo.SetActive(false);
        panelOpciones.SetActive(false);
    }


    public void activarDIalogo()
    {
        //Debug.Log("Clic en UI para iniciar di�logo");
        managerDialogo manager = FindObjectOfType<managerDialogo>();
        if (manager != null)
        {
            Debug.Log("El manager dialogo no es null");
            manager.IniciarDialogo(dialogo);
        } else Debug.Log("El manager dialogo es null, no se puede abrir");
    }
}
