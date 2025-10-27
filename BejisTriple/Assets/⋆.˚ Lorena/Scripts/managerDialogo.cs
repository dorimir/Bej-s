using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class managerDialogo : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI dialogoTexto;
    public Image imagenPJ;
    public GameObject panelOpciones;

    [Header("Bloqueo de interacción")]
    public GameObject bloqueadorInput;

    private dialogoSO dialogoActual;
    private opcionDialogo opcionDialogo;
    private int indiceLineaActual = 0;
    private bool dialogoActivo = false;

    void Update()
    {
        if (dialogoActivo && Input.GetKeyDown(KeyCode.E) ||
            dialogoActivo && Input.GetKeyDown(KeyCode.Space))
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

    void TerminarDialogo()
    {
        dialogoActivo = false;
        panelDialogo.SetActive(false);
        bloqueadorInput.SetActive(false); 
        panelOpciones.SetActive(true);
    }
}
