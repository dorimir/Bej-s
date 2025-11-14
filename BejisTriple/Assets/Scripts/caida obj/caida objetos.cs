using UnityEngine;
using System.Collections;

public class caidaobjetos : MonoBehaviour
{
    public GameObject Herradura;
    public GameObject caballo;
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
        Vector3 pos = new Vector3(Random.Range(-2, 12), 8, 0);
        int azar = Random.Range(0,2);
            switch (azar)
            {
                case 0:
                    Instantiate(caballo, pos, Quaternion.identity);

                    break;
                case 1:
                    Instantiate(Herradura, pos, Quaternion.identity);
                    break;
                default: break;
            }
        yield return new WaitForSeconds(delay);
        
    }
    }
}


