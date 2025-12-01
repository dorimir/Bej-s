using UnityEngine;

public class NPCSllegan : MonoBehaviour
{
    public GameObject[] lista;
    public int[] escenaEnLaQueLlegan;

    void Start()
    {
        for(int i = 0; i<lista.Length; i++)
        {
            if(GameManager.Instance.ContadorDeMinijuegos()<escenaEnLaQueLlegan[i])
            {
                lista[i].SetActive(false);
            }
        }
    }
}
