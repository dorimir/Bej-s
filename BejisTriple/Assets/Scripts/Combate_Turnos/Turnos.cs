using UnityEngine;


public class Turnos : MonoBehaviour
{
    private Class_Fighter Personaje_1;
    private Class_Fighter Personaje_2;
    private bool yetToShow = false;

    public Sprite J1;
    public Sprite J2;

    public int tipo_enemigo;
    public int tipo_jugador;
    public bool JvJ;

    private int[] stats = { 25, 10, 10, 25, 15, 20 };
    private enum Estado { START, J1TURN, J2TURN, ENDED};
    private Estado estado;

    public bool Acabado() // editar con más formas de que acabe el combate
    {
        if (estado == Estado.ENDED)
        {
            return true;
        }
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        estado = Estado.START;
    }

    public void Iniciar(bool JugadorVsJugador, int tipo_e, int tipo_j, Sprite J, Sprite E)
    {
        estado = Estado.START;
        JvJ = JugadorVsJugador;
        tipo_enemigo = tipo_e;
        tipo_jugador = tipo_j;
        J1 = J;
        J2 = E;
        Preparar_Batalla();
    }

    void Preparar_Batalla()
    {
        GameObject jugadorObj = new GameObject("Jugador");
        Personaje_1 = jugadorObj.AddComponent<Class_Fighter>();
        SpriteRenderer renderer_1 = jugadorObj.AddComponent<SpriteRenderer>();
        if (!JvJ)
        {
            Personaje_1.Stats(stats[4], stats[5]);
        }
        else if (tipo_jugador == 1)
        {
            Personaje_1.Stats(stats[0], stats[1]);
        }
        else if (tipo_jugador == 2)
        {
            Personaje_1.Stats(stats[2], stats[3]);
        }
        else
        {
            Personaje_1.Stats(stats[4], stats[5]);
        }
        renderer_1.sprite = J1;
        jugadorObj.transform.position = new Vector3(-2, 0, 0);

        GameObject enemigoObj = new GameObject("Enemigo");
        Personaje_2 = enemigoObj.AddComponent<Class_Fighter>();
        SpriteRenderer renderer_2 = enemigoObj.AddComponent<SpriteRenderer>();
        if (tipo_enemigo == 1)
        {
            Personaje_2.Stats(stats[0], stats[1]);
        } else if (tipo_enemigo == 2)
        {
            Personaje_2.Stats(stats[2], stats[3]);
        } else
        {
            Personaje_2.Stats(stats[4], stats[5]);
        }
        renderer_2.sprite = J2;
        enemigoObj.transform.position = new Vector3(2, 0, 0);
        Mostrar_Guardias();
        estado = Estado.J1TURN;
    }

    void Mostrar_Guardias()
    {
        Finalizado();
        if (Personaje_1 != null)
            Debug.Log("J1 Guardia: " + Personaje_1.GetGuardia()); // Cambiar cuando gráficos
        if (Personaje_2 != null)
            Debug.Log("J2 Guardia: " + Personaje_2.GetGuardia()); // Cambiar cuando gráficos
    }

    void Finalizado()
    {
        if (Personaje_1 == null)
        {
            estado = Estado.ENDED;
            Debug.Log("J2 gana!"); // Cambiar cuando gráficos
        }
        if (Personaje_2 == null)
        {
            estado = Estado.ENDED;
            Debug.Log("J1 gana!"); // Cambiar cuando gráficos
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!yetToShow && estado != Estado.ENDED) { Mostrar_Guardias(); yetToShow = true; }
        if (estado == Estado.J1TURN)
        {
            if (Input.GetKeyDown(KeyCode.A)){
                Personaje_1.Ataque(Personaje_2);
                yetToShow = false;
                estado = Estado.J2TURN;
            }
            if (Input.GetKeyDown(KeyCode.D)){
                Personaje_1.Defender();
                yetToShow = false;
                estado = Estado.J2TURN;
            }
            if (Input.GetKeyDown(KeyCode.F)){
                Personaje_1.Finalizar(Personaje_2);
                yetToShow = false;
                estado = Estado.J2TURN;
            }
        } else if (estado == Estado.J2TURN && JvJ)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                Personaje_2.Ataque(Personaje_1);
                yetToShow = false;
                estado = Estado.J1TURN;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Personaje_2.Defender();
                yetToShow = false;
                estado = Estado.J1TURN;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Personaje_2.Finalizar(Personaje_1);
                yetToShow = false;
                estado = Estado.J1TURN;
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
