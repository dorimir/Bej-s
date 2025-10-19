using UnityEngine;
using UnityEngine.EventSystems; 

public class activarDialogo : MonoBehaviour, IPointerClickHandler
{
    public dialogoSO dialogo;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clic en UI para iniciar diálogo");
        managerDialogo manager = FindObjectOfType<managerDialogo>();
        if (manager != null)
        {
            manager.IniciarDialogo(dialogo);
        }
    }
}

