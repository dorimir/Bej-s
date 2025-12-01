using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour, IInteractable
{
    public string SceneName;

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
    }
}