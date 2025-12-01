using UnityEngine;
using System.Collections;
using System.Collections.Generic;



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
    int bueno;
    int malo;


    public int cantidad = 10;
    float delay = 1;
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
        List<GameObject> objetosActivos = new List<GameObject>();
        yield return new WaitForSeconds(2);
        for(int i = 0; i <= cantidad; i++)
        {
            azar = Random.Range(0,4);
            if(azar!=2) pos = new Vector3(Random.Range(-2, 10), 8, 0);
            else pos = new Vector3(player.transform.position.x, 8, 0);
            
            if(bueno==2) azar = Random.Range(2,4);
            if(malo==2)  azar =Random.Range(0,2);

            
            switch (azar)
            {
                case 0:
                    obj=Instantiate(caballo, pos, Quaternion.identity);
                    objetosActivos.Add(obj);
                    malo=0;
                    bueno++;
                    break;
                case 1:
                    obj=Instantiate(Herradura, pos, Quaternion.identity);
                    objetosActivos.Add(obj);
                    malo=0;
                    bueno++;
                    break;
                case 2:
                    obj=Instantiate(espada, pos, Quaternion.Euler(0, 0, 176));
                    objetosActivos.Add(obj);
                    bueno=0;
                    malo++;
                    break;
                case 3:
                    obj=Instantiate(Heces, pos, Quaternion.identity);
                    objetosActivos.Add(obj);
                    bueno=0;
                    malo++;
                    break;
                default: break;
            }
            yield return new WaitForSeconds(delay);
            
        }
        while (objetosActivos.Count > 0)
        {
            objetosActivos.RemoveAll(o => o == null);
            yield return null;
        }        
        juegoacabado();
        
    }
    public void juegoacabado(){
        Juego.SetActive(false);
        int erroresTOTALES=Puntoserrores.GetComponent<Puntoserrores>().errorescometidos;
        if(erroresTOTALES>=3){
            pantallaDerrota.SetActive(true);
        }
        else {
            pantallaVictoria.SetActive(true);
            GameManager.Instance.minijuegoCompletado(1);
        }
    }
}


