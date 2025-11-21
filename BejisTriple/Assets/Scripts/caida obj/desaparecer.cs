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

/*void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == surface)
        {
            switch(transform.GetChild(1).gameObject.tag)
            {
                case "Trucha":
                        TimeAndScore.GetComponent<TimeAndScore>().AddScore(1);
                    break;
                case "Carpa":
                        TimeAndScore.GetComponent<TimeAndScore>().AddScore(3);
                    break;
                case "Siluro":
                        TimeAndScore.GetComponent<TimeAndScore>().AddScore(7);
                    break;
                case "Barbo":
                        TimeAndScore.GetComponent<TimeAndScore>().AddScore(12);
                    break;
                default:
                        break;
            }
            Destroy(transform.GetChild(1).gameObject);
            HasSomethingHooked = false;
        }
    }
*/

