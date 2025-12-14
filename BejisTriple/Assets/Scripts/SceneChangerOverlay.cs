using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : MonoBehaviour
{
    // Must be public, void, no parameters
    public void Continue()
    {
        SceneManager.LoadScene("Intro_Cutscene");
    }
    public void configuration(){
        SceneManager.LoadScene("Configuraciónmenu");
        Debug.Log("Lol");
    }
    public void Menu(){
        SceneManager.LoadScene(1);
    }
    public void Salir(){
        Application.Quit();
    }
}