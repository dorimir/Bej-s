using UnityEngine;

public class Recogerobjetos : MonoBehaviour
{
    public GameObject Puntoserrores;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.collider.CompareTag("ropacaballo") || collision.collider.CompareTag("herraduramin") 
        || collision.collider.CompareTag("heces") || collision.collider.CompareTag("espada"))
        {
            switch(collision.collider.tag){
            case "ropacaballo":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().ptos(100);
                break;
            case "herraduramin":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().ptos(200);
                break;
            case "heces":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().errores(1);
                break;
            case "espada":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().errores(3);

                break;
            default: break;
        }
        }
    }
}
