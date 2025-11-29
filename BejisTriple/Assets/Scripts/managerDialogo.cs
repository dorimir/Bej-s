using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class managerDialogo : MonoBehaviour
{

    //Este archivo es el que lleva el manager de dialogos, hay uno en cada escena
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI dialogoTexto;
    public Image imagenPJ;
    public GameObject panelOpciones;

    [Header("Bloqueo de interacci�n")]
    public GameObject bloqueadorInput;

    [Header("Personaje con el que se habla")]
    public GameObject npcActual;

    public dialogoSO dialogoActual;
    private opcionDialogo opcionDialogo;
    public int indiceLineaActual = 0;
    public bool dialogoActivo = false;



    void Update()
    {
        if (dialogoActivo && Input.GetKeyDown(KeyCode.Space))
        {
            MuestraSiguienteLinea();
        }
    }

    public void IniciarDialogo(dialogoSO nuevoDialogo)
    {
        if (nuevoDialogo == null || nuevoDialogo.lineas.Length == 0) return;

        bloqueadorInput.SetActive(true);
        panelOpciones.SetActive(false);

        dialogoActual = nuevoDialogo;
        indiceLineaActual = 0;
        dialogoActivo = true;
        panelDialogo.SetActive(true);
        MostrarLinea(dialogoActual.lineas[indiceLineaActual]);

    }

    void MuestraSiguienteLinea()
    {
        indiceLineaActual++;

        if (indiceLineaActual < dialogoActual.lineas.Length)
        {
            MostrarLinea(dialogoActual.lineas[indiceLineaActual]);
        }
        else
        {
            TerminarDialogo();
        }
    }

    void MostrarLinea(lineaDialogo linea)
    {
        nombreTexto.text = dialogoActual.nombrePJ;
        dialogoTexto.text = linea.textoDialogo;
        imagenPJ.sprite = linea.expresionPJ;
    }

    public void TerminarDialogo()
    {
        dialogoActivo = false;
        panelDialogo.SetActive(false);
        bloqueadorInput.SetActive(false); 
        if(GameManager.Instance.ContadorDeMinijuegos()== npcActual.GetComponent<activarOpciones>().variableConOpciones)
                    {
                        Debug.Log("La variable en la que debería haber minijuego es: " + npcActual.GetComponent<activarOpciones>().variableConOpciones);
                        Debug.Log("El valor de la variable en el game manager es: " + GameManager.Instance.ContadorDeMinijuegos());
                        npcActual.GetComponent<activarOpciones>().MostrarOpciones();
                    }
        npcActual.GetComponent<activarOpciones>().haEmpezadoDialogo = false;
        npcActual = null;
        //panelOpciones.SetActive(true);
    }
}
