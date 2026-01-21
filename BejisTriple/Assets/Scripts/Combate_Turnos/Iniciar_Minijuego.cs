using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Iniciar_Minijuego : MonoBehaviour
{

    public Sprite[] sprites;

    public dialogoSO[] Dialogo_Entrada;

    public dialogoSO[] Dialogo_Derrota;

    public TMP_Text GJ;
    public TMP_Text GE;

    public GameObject canvas;

    private bool haPerdidoAnterior;

    public managerDialogoCombate manager;

    private int tipo_enemigo;

    private GameObject combate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tipo_enemigo = 0;
        haPerdidoAnterior = false;
        canvas.SetActive(true);
        CrearCombate();
    }

    void CrearCombate()
    {
        if (tipo_enemigo >= 3)
        {
            GameManager.Instance.minijuegoCompletado(5);
            SceneTransitionManager.Instance.LoadScene("Final_Cutscene 1");
            Debug.Log("Terminado"); return;
        }
        Debug.Log("Creando Combate");
        combate = new GameObject("Combate: " + tipo_enemigo);
        Turnos codigo = combate.AddComponent<Turnos>();

        codigo.Iniciar(tipo_enemigo, sprites[0], sprites[tipo_enemigo + 1], GJ, GE);

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
