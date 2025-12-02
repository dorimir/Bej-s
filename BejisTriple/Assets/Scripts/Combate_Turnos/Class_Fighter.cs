using UnityEngine;

public class Class_Fighter : MonoBehaviour
{
    int Def;
    int Atq;
    int Guardia;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Guardia = 100;
    }

    public void Stats(int Defensa, int Ataque)
    {
        Def = Defensa;
        Atq = Ataque;
    }

    public int GetGuardia() { return Guardia; }

    void Reducir_Guardia(int cantidad)
    {
        Guardia -= cantidad;
    }

    public void Ataque(Class_Fighter enemigo)
    {
        enemigo.Reducir_Guardia(Atq);
        Debug.Log("Atacar!");
    }

    public void Defender() { 
        Guardia += Def;
        if (Guardia > 100) Guardia = 100;
        Debug.Log("Defender!");
    }

    bool Probar_Guardia(int roll)
    {
        return Guardia < roll;
    }

    public void Finalizar(Class_Fighter enemigo)
    {
        Reducir_Guardia(Atq);
        if (enemigo.Probar_Guardia(Random.Range(1,100))){
            enemigo.Derrotar();
        }
        Debug.Log("Finalizar!");
    }

    void Derrotar()
    {
        Destroy(gameObject);
    }

    public void Tomar_Decision(Class_Fighter enemigo, int tipo)
    {
        if (tipo == 1)
        {
            if (enemigo.GetGuardia() < 35) {
                Guardia -= Atq;
                Finalizar(enemigo);
            } else if(Guardia < 60) {
                Defender();
            } else {
                Ataque(enemigo);
            }
         } else if (tipo == 2) {
            if (enemigo.GetGuardia() < 65) {
                Guardia -= Atq;
                Finalizar(enemigo);
            } else if (Guardia < 25) {
                Defender();
            } else {
                Ataque(enemigo);
            }
         } else
         {
            if (enemigo.GetGuardia() < 50)
            {
                Guardia -= Atq;
                Finalizar(enemigo);
            }
            else if (Guardia < 45)
            {
                Defender();
            }
            else
            {
                Ataque(enemigo);
            }
        }

    }
}
