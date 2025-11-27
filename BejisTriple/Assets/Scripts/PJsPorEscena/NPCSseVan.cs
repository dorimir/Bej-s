using UnityEngine;

public class NPCSseVan : MonoBehaviour
{
    public GameObject[] lista;
    public int[] escenaEnLaQueSeVan;

    void Start()
    {
        for(int i = 0; i<lista.Length; i++)
        {
            if(GameManager.Instance.ContadorDeMinijuegos()>=escenaEnLaQueSeVan[i])
            {
                lista[i].SetActive(false);
            }
        }
    }
}
