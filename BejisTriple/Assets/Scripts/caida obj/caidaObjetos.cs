using UnityEngine;
using System.Collections;


public class caidaObjetos : MonoBehaviour
{
    public GameObject Herradura;
    public GameObject Heces;
    public GameObject caballo;
    public GameObject espada;

    public GameObject player;

    public GameObject Juego;
    public GameObject pantallaVictoria;
    public GameObject pantallaDerrota;

    public GameObject Puntoserrores;


    

    Vector3 pos;
    int azar;
    private GameObject obj;

    public int cantidad = 10;
    int delay = 3;
    public Vector2 rangoX = new Vector2(-5f, 5f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        StartCoroutine(spawnerobj());
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnerobj(){
        yield return new WaitForSeconds(delay-1);
        for(int i = 0; i <= cantidad; i++)
        {
            azar = Random.Range(0,4);
            if(azar!=2) pos = new Vector3(Random.Range(-2, 10), 8, 0);
            else pos = new Vector3(player.transform.position.x, 8, 0);
            
            
            
            switch (azar)
            {
                case 0:
                    obj=Instantiate(caballo, pos, Quaternion.identity);
                    break;
                case 1:
                    obj=Instantiate(Herradura, pos, Quaternion.identity);
                    break;
                case 2:
                    obj=Instantiate(espada, pos, Quaternion.Euler(0, 0, 176));
                    break;
                case 3:
                    obj=Instantiate(Heces, pos, Quaternion.identity);
                    break;
                default: break;
            }
            yield return new WaitForSeconds(delay);
            
        }
        while (obj != null) yield return null;
        
    }
    public void juegoacabado(){
        Juego.SetActive(false);
        int erroresTOTALES=Puntoserrores.GetComponent<Puntoserrores>().errorescometidos;
        if(erroresTOTALES>=3){
            pantallaDerrota.SetActive(true);
        }
        else pantallaVictoria.SetActive(true);
    }
}


