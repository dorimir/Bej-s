using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Arabe_activarOpciones : activarOpciones
{
    protected override void Update()
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

}
