using UnityEngine;
using System;

public class Iniciar_Minijuego : MonoBehaviour
{

    public Sprite[] sprites;

    public dialogoSO[] Dialogo_Entrada;

    public dialogoSO[] Dialogo_Derrota;

    private bool haPerdidoAnterior;

    public managerDialogoCombate manager;

    private int tipo_enemigo;

    private GameObject combate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tipo_enemigo = 0;
        haPerdidoAnterior = false;
        CrearCombate();
    }

    void CrearCombate()
    {
        if (tipo_enemigo >= 3)
        {
            Debug.Log("Terminado"); return;
        }
        Debug.Log("Creando Combate");
        combate = new GameObject("Combate: " + tipo_enemigo);
        Turnos codigo = combate.AddComponent<Turnos>();

        codigo.Iniciar(tipo_enemigo, sprites[0], sprites[tipo_enemigo + 1]);

        if (!haPerdidoAnterior)
        {
            Debug.Log("Combate ganado.");
            manager.IniciarDialogo(Dialogo_Entrada[tipo_enemigo]);
        } else
        {
            Debug.Log("Combate perdido.");
            manager.IniciarDialogo(Dialogo_Derrota[tipo_enemigo]);
        }

        haPerdidoAnterior = true;

        codigo.FinalCombate += () =>
        {
            if (codigo.ganaJugador())
            {
                tipo_enemigo += 1;
                haPerdidoAnterior = false;
            }
            codigo.Destruir_Todo();
            Destroy(combate);
            Debug.Log("Combate iniciado");
            combate = null;
            CrearCombate();
        };
        
    }
}
