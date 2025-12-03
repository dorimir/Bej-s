using UnityEngine;
using UnityEngine.SceneManagement;

public class introScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "MainMenuScene"; // Cambia el nombre aquí

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}