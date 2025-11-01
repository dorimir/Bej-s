using UnityEngine;
using UnityEngine.SceneManagement;

public class numbergenerator : MonoBehaviour, IInteractable{
    GameObject Jugador;
    public void Interact(){
        SceneManager.LoadScene(2);
        
    }

}