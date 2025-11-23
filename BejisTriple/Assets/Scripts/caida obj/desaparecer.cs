using UnityEngine;

public class desaparecer : MonoBehaviour
{
    public GameObject obj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("espada") || (collision.collider.CompareTag("heces")))
        {
            Destroy(collision.gameObject);
        }
        switch(collision.collider.tag){
            case "ropacaballo":
                Destroy(collision.gameObject);
                break;
            case "herraduramin":

                Destroy(collision.gameObject);
                break;
            default: break;
        }
    }
}


