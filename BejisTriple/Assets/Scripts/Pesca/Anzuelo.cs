using UnityEngine;

public class Anzuelo : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    public float speed = 1;
    Rigidbody body;
    //Si llevamos un pez a la superficie, lo quitamos del anzuelo y podemos seguir pescando
    public GameObject surface;

    public GameObject TimeAndScore;
    public FishSpawner fishSpawner;
    public AudioSource audioSource;

    bool HasSomethingHooked = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == surface)
        {
            fishSpawner.contadorPeces--;
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
            audioSource.clip = audioSource.gameObject.GetComponent<SFXManagerPesca>().sacarDelAgua;
            audioSource.Play();
            Destroy(transform.GetChild(1).gameObject);
            HasSomethingHooked = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (HasSomethingHooked == true) return;
        if(collision.tag == "Trucha" || collision.tag == "Carpa" || collision.tag == "Siluro" || collision.tag == "Barbo" || collision.tag == "BasuraDeRio")
        {
            audioSource.clip = audioSource.gameObject.GetComponent<SFXManagerPesca>().enganchar;
            audioSource.Play();
            collision.transform.SetParent(this.transform);
            collision.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            collision.GetComponent<Rigidbody>().linearVelocity = new UnityEngine.Vector2(0, 0);
            HasSomethingHooked = true;
        }
    }
}
