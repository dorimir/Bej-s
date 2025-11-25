using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour, IInteractable
{
    public string SceneName;
    public void Interact()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
