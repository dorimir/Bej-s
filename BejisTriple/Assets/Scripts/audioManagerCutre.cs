using UnityEngine;

public class audioManagerCutre : MonoBehaviour
{
    [SerializeField] private AudioClip fondoMusica;
    [SerializeField] private AudioSource musicAudioSource;

    private void Start()
    {
        musicAudioSource.ignoreListenerPause = true;
        musicAudioSource.clip = fondoMusica;
        musicAudioSource.Play();
    }
}
