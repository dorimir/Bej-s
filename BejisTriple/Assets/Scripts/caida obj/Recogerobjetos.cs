using UnityEngine;

public class Recogerobjetos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.collider.CompareTag("Objetoscaen"))
        {
            Destroy(other.gameObject);
            Debug.Log("+1");
        }
    }
}
