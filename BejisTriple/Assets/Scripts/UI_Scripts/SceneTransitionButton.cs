using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime;
    [SerializeField] private int nextSceneIndex;

    void Start()
    {
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(Transition(nextSceneIndex));
    }

    public IEnumerator Transition(int sceneIndex)
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);

        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}