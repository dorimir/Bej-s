using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController instance;

    [SerializeField] private Animator transition;

    private void Awake()
    {
        // Singleton básico
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Opcional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    private IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
        transition.SetTrigger("Start");
    }
}