using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class activarOpciones : MonoBehaviour
{
    //Este archivo lo tiene cada NPC por su cuenta
    public GameObject panelOpciones;

    [Header("Referencias dialogoSO")]
    public dialogoSO opcionUno;
    public dialogoSO opcionDos;
    public dialogoSO opcionTres;

    [Header("Referencias UI")]
    public TextMeshProUGUI nombreOpcionUno;
    public TextMeshProUGUI nombreOpcionDos;
    public TextMeshProUGUI nombreOpcionTres;

    [Header("Referencias Boton")]

    public Button botonOpcionUno;
    public Button botonOpcionDos;
    public Button botonOpcionTres;

    [Header("Referencia a managerDialogo")]
    public managerDialogo manager;

    protected bool jugadorDentro = false;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            Debug.Log("Jugador ha entrado en el área de interacción.");
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            panelOpciones.SetActive(false);

            jugadorDentro = false;
            Debug.Log("Jugador ha salido del área de interacción.");
        }
    }

    protected virtual void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Space))
        {
            if (manager != null && manager.dialogoActivo) 
            {
                Debug.Log("No se pueden mostrar opciones: diálogo activo.");
                return;
            }

            MostrarOpciones();
        }
    }

    public virtual void MostrarOpciones()
    {
        Debug.Log("Clic en UI");
        panelOpciones.SetActive(true);

        nombreOpcionUno.text = opcionUno.nombreOpcion;
        nombreOpcionDos.text = opcionDos.nombreOpcion;
        nombreOpcionTres.text = opcionTres.nombreOpcion;

        var opcionScriptUno = botonOpcionUno.GetComponent<opcionDialogo>();
        opcionScriptUno.dialogo = opcionUno;

        var opcionScriptDos = botonOpcionDos.GetComponent<opcionDialogo>();
        opcionScriptDos.dialogo = opcionDos;

        var opcionScriptTres = botonOpcionTres.GetComponent<opcionDialogo>();
        opcionScriptTres.dialogo = opcionTres;
    }

}
