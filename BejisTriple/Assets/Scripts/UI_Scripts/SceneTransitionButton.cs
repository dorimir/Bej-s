using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Button activationButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (activationButton != null)
        {
            activationButton.onClick.AddListener(LoadNextScene);
        }
    }


    public void LoadNextScene()
    {
        StartCoroutine(Transition(nextSceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    public IEnumerator Transition(string sceneName)
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}