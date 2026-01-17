using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour, IInteractable
{
    public string SceneName;
    public AudioClip correr, puerta;
    public string destinationSpawnID;

    public void Interact()
    {
        GameManager.nextSpawnID = destinationSpawnID;
        GameManager.originalPlayerScale = GameObject.FindGameObjectWithTag("Player").transform.localScale;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar la puerta de destino antes de cambiar de escena
            GameManager.nextSpawnID = destinationSpawnID;
        }
    }
}