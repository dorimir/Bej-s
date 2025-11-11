using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour
{
    public string SceneName;
    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player") //Esto lo cambiaremos cuando estén impl
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }
}
