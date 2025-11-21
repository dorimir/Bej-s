using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class opcionDialogo : MonoBehaviour
{
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
        //Debug.Log("Clic en UI para iniciar diálogo");
        managerDialogo manager = FindObjectOfType<managerDialogo>();
        if (manager != null)
        {
            manager.IniciarDialogo(dialogo);
        }
    }
}
