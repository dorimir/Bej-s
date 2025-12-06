using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour, IInteractable
{
    public string SceneName;
    public AudioClip correr, puerta;

    public void Interact()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(SceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
        if(GetComponent<SpriteRenderer>().sprite.name == "flecha")
            {
                GameManager.Instance.sonidoCambiarEscena(correr);
            }else
            {
                GameManager.Instance.sonidoCambiarEscena(puerta);
            }
    }
}