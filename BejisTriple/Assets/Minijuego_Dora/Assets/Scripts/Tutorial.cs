using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private float delayBeforeTransition = 5f; // set your delay in seconds

    private void Start()
    {
        StartCoroutine(TransitionAfterDelay());
    }

    private IEnumerator TransitionAfterDelay()
    {
        // Wait for the chosen delay
        yield return new WaitForSeconds(delayBeforeTransition);

        // Load Scene 1 (by name or build index)
        SceneManager.LoadScene("TiroConArco");
        // Or: SceneManager.LoadScene("Scene1"); if you prefer using the scene name
    }
}