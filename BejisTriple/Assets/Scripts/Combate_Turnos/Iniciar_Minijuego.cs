using UnityEngine;

public class Iniciar_Minijuego : MonoBehaviour
{

    public Sprite jugador;
    public Sprite Defensivo;
    public Sprite Atacante;
    public Sprite Equilibrado;

    public bool JvJ;

    private int tipo_enemigo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tipo_enemigo = 1;
        CrearCombate();
    }

    void CrearCombate()
    {
        if (tipo_enemigo < 4)
        {
            Debug.Log("Terminado"); return;
        }
        GameObject nuevoCombate = new GameObject("Combate_{tipo_enemigo + 1}");
        
    }
}
