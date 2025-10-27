using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class activarOpciones : MonoBehaviour, IPointerClickHandler
{
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

    public void OnPointerClick(PointerEventData eventData)
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
