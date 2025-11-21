using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject canvatutorial;  // Panel con las instrucciones
    public GameObject juego;

    public void StartGame(){
        canvatutorial.SetActive(false);
        juego.SetActive(true);
    }
    public void restart(){
        SceneManager.LoadScene(2);
    }
}
