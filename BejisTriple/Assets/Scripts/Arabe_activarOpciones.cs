using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Arabe_activarOpciones : activarOpciones
{
    public Button botonIniciarMinijuego;
    bool haEmpezadoDialogo = false;
    protected override void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Space))
        {
            if(manager!= null)
            {
                if (!manager.dialogoActivo) 
                {
                    if(!haEmpezadoDialogo)
                    {
                        manager.IniciarDialogo(opcionUno);
                        haEmpezadoDialogo = true;
                    }else MostrarOpciones();
                }
            }
        }
    }
    public override void MostrarOpciones()
    {
        Debug.Log("Clic en UI");
        panelOpciones.SetActive(true);
        botonOpcionUno.gameObject.SetActive(false);

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
