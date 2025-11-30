using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class activarOpciones : MonoBehaviour
{
    //Este archivo lo tiene cada NPC por su cuenta
    public GameObject panelOpciones;

    [Header("Referencias dialogoSO")]
    public dialogoSO var0, var1, var2, var3, var4;
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
    public Button botonIniciarMinijuego;

    [Header("Referencia a managerDialogo")]
    public managerDialogo manager;

    protected bool jugadorDentro = false;
    

    [Header("Otras variables")]
    public int variableConOpciones;
    public bool tieneMinijuego = false;
    public string minijuego;
    public bool haEmpezadoDialogo = false;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            panelOpciones.SetActive(false);

            jugadorDentro = false;
            manager.npcActual = null;
            haEmpezadoDialogo = false;
        }
    }

    protected virtual void Update()
    {
        /*La funcion que habia en el activar opciones original de Lorena
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Space))
        {
            if (manager != null && manager.dialogoActivo) 
            {
                Debug.Log("No se pueden mostrar opciones: diálogo activo.");
                return;
            }

            MostrarOpciones();
        }*/
        /*
        Una funcion que añadio Fer mas tarde pero que se mejoró a la que hay abajo
        if (!manager.dialogoActivo) 
                {
                    if(!haEmpezadoDialogo)
                    {
                        manager.IniciarDialogo(opcionUno);
                        haEmpezadoDialogo = true;
                    }else MostrarOpciones();
                }
        */
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Space))
        {
            if(manager!= null && !manager.dialogoActivo)
            {
                manager.npcActual = gameObject;
                Debug.Log("El npc actual es " + manager.npcActual);
                if(!haEmpezadoDialogo)
                {
                    switch(GameManager.Instance.ContadorDeMinijuegos()) 
                    {
                    case 1:
                        manager.IniciarDialogo(var1);
                        break;
                    case 2:
                        manager.IniciarDialogo(var2);
                        break;
                    case 3:
                        manager.IniciarDialogo(var3);
                        break;
                    case 4:
                        manager.IniciarDialogo(var4);
                        break;
                    default:
                        manager.IniciarDialogo(var0);
                        break;
                    }
                    haEmpezadoDialogo = true;
                }
            }
        }
    }

    public virtual void MostrarOpciones()
    {
        Debug.Log("Clic en UI");
        panelOpciones.SetActive(true);
        if(tieneMinijuego)
        {
            botonOpcionUno.gameObject.SetActive(false);
            botonIniciarMinijuego.GetComponent<opcionDialog_Minijuego>().Minijuego = minijuego;
        }else
        {
            botonIniciarMinijuego.gameObject.SetActive(false);
        }
        nombreOpcionUno.text = opcionUno.nombreOpcion;
        nombreOpcionDos.text = opcionDos.nombreOpcion;

        var opcionScriptUno = botonOpcionUno.GetComponent<opcionDialogo>();
        opcionScriptUno.dialogo = opcionUno;

        var opcionScriptDos = botonOpcionDos.GetComponent<opcionDialogo>();
        opcionScriptDos.dialogo = opcionDos;

        if(opcionTres==null)
        {
            botonOpcionTres.gameObject.SetActive(false);
        }
        else
        {
            nombreOpcionTres.text = opcionTres.nombreOpcion;
            var opcionScriptTres = botonOpcionTres.GetComponent<opcionDialogo>();
            opcionScriptTres.dialogo = opcionTres;
        }
    }

}
