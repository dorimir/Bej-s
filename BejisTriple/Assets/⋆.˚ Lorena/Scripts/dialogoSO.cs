using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Dialogos/DialogoCompleto")]


public class dialogoSO : ScriptableObject
{
    public string nombrePJ;

    public string nombreOpcion;

    public lineaDialogo[] lineas;
}
