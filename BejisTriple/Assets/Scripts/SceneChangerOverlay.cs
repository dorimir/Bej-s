using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : MonoBehaviour
{
    // Must be public, void, no parameters
    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
}