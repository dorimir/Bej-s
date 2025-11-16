using UnityEngine;
using System.Collections;

public class caidaobjetos : MonoBehaviour
{
    public GameObject Herradura;
    public GameObject Heces;
    public GameObject caballo;
    public GameObject espada;

    public GameObject player;

    Vector3 pos;
    int azar;

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
    for(int i = 0; i <= cantidad; i++)
    {
        azar = Random.Range(0,4);
        azar=2;
        if(azar!=2) pos = new Vector3(Random.Range(-2, 12), 8, 0);
        else pos = new Vector3(player.transform.position.x, 8, 0);
        
        
        
        switch (azar)
        {
            case 0:
                Instantiate(caballo, pos, Quaternion.identity);

                break;
            case 1:
                Instantiate(Herradura, pos, Quaternion.identity);
                break;
            case 2:
                Instantiate(espada, pos, Quaternion.Euler(0, 0, 176));
                break;
            case 3:
                Instantiate(Heces, pos, Quaternion.identity);
                break;
            default: break;
        }
        yield return new WaitForSeconds(delay);
        
    }
    }

    GameObject[] objscaen = GameObject.FindGameObjectsWithTag("Objetoscaen");
    GameObject[] objscaenmalos = GameObject.FindGameObjectsWithTag("Caenmalos");
}


