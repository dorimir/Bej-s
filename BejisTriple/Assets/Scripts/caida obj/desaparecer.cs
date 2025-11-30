using UnityEngine;

public class desaparecer : MonoBehaviour
{
    public GameObject Puntoserrores;

 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("espada") || (collision.collider.CompareTag("heces")))
        {
            Destroy(collision.gameObject);
        }
        switch(collision.collider.tag){
            case "ropacaballo":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().errores(1);
                
                break;
            case "herraduramin":
                Destroy(collision.gameObject);
                Puntoserrores.GetComponent<Puntoserrores>().errores(1);
                break;
            default: break;
        }
    }
}


