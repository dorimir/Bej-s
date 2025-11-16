using UnityEngine;

public class tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject canvatutorial;  // Panel con las instrucciones
    public GameObject juego;

    public void StartGame(){
        canvatutorial.SetActive(false);
        juego.SetActive(true);
    }
}
