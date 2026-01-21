using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Turnos : MonoBehaviour
{
    private Class_Fighter Personaje_1;
    private Class_Fighter Personaje_2;
    private bool yetToShow = false;

    private float delay = 1f;

    public Action FinalCombate;

    public Sprite J1;
    public Sprite J2;

    public TMP_Text Guardia_Jugador;
    public TMP_Text Guardia_Enemigo;

    public int tipo_enemigo;

    private int[] stats = { 30, 10, 10, 25, 20, 20 };
    private enum Estado { START, J1TURN, J2TURN, ENDED};
    private Estado estado;

    public bool ganaJugador() // editar con más formas de que acabe el combate
    {
        if (Personaje_1 == null)
        {
            return false;
        }
        return true;
    }

    public void Destruir_Todo()
    {
        if (Personaje_1 != null) Destroy(Personaje_1.gameObject);
        if (Personaje_2 != null) Destroy(Personaje_2.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        estado = Estado.J1TURN;
    }

    public void Iniciar(int tipo_e, Sprite J, Sprite E, TMP_Text GJ, TMP_Text GE)
    {
        estado = Estado.START;
        tipo_enemigo = tipo_e;
        J1 = J;
        J2 = E;
        Guardia_Jugador = GJ;
        Guardia_Enemigo = GE;
        Preparar_Batalla();
    }

    void Preparar_Batalla()
    {
        GameObject jugadorObj = new GameObject("Jugador");
        Personaje_1 = jugadorObj.AddComponent<Class_Fighter>();
        SpriteRenderer renderer_1 = jugadorObj.AddComponent<SpriteRenderer>();
        renderer_1.sortingOrder = 5;

        Personaje_1.Stats(stats[4], stats[5]);

        renderer_1.sprite = J1;
        jugadorObj.transform.position = new Vector3(-5, 2.84f, -5);

        GameObject enemigoObj = new GameObject("Enemigo");
        Personaje_2 = enemigoObj.AddComponent<Class_Fighter>();
        SpriteRenderer renderer_2 = enemigoObj.AddComponent<SpriteRenderer>();
        //{ 25, 10, 10, 25, 15, 20 };
        if (tipo_enemigo == 0)
        {
            Personaje_2.Stats(stats[0], stats[1]);
        } else if (tipo_enemigo == 1)
        {
            Personaje_2.Stats(stats[2], stats[3]);
        } else
        {
            Personaje_2.Stats(stats[4], stats[5]);
        }
        renderer_2.sprite = J2;
        enemigoObj.transform.position = new Vector3(5.3f, 2.84f, -5);
        if (tipo_enemigo == 0)
        {
            enemigoObj.transform.position = new Vector3(5.3f, 3.24f, -5);
        }
        enemigoObj.transform.localScale = new Vector3(-1, 1, 1);
        renderer_2.sortingOrder = 5;

        estado = Estado.J1TURN;
    }

    void Mostrar_Guardias()
    {
        Finalizado();
        if (Personaje_1 != null && Personaje_2 != null)
        {
            Debug.Log("J1 Guardia: " + Personaje_1.GetGuardia()); // Cambiar cuando gráficos
            Guardia_Jugador.text = "Guardia Jugador: " + Personaje_1.GetGuardia();
            Debug.Log("J2 Guardia: " + Personaje_2.GetGuardia()); // Cambiar cuando gráficos
            Guardia_Enemigo.text = "Guardia Enemigo: " + Personaje_2.GetGuardia();
        }
    }

    void Finalizado()
    {
        if (Personaje_1 == null)
        {
            estado = Estado.ENDED;
            Debug.Log("J2 gana!"); // Cambiar cuando gráficos
            FinalCombate?.Invoke();
        }
        if (Personaje_2 == null)
        {
            estado = Estado.ENDED;
            Debug.Log("J1 gana!"); // Cambiar cuando gráficos
            FinalCombate?.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!yetToShow && estado != Estado.ENDED) { Mostrar_Guardias(); yetToShow = true; }
        if (estado == Estado.J1TURN)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                Personaje_1.Ataque(Personaje_2);
                yetToShow = false;
                estado = Estado.J2TURN;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Personaje_1.Defender();
                yetToShow = false;
                estado = Estado.J2TURN;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Personaje_1.Finalizar(Personaje_2);
                yetToShow = false;
                estado = Estado.J2TURN;
            }
        }
        else if (estado == Estado.J2TURN)
        {
            Personaje_2.Tomar_Decision(Personaje_1, tipo_enemigo);
            yetToShow = false;
            estado = Estado.J1TURN;
        }
    }
}
