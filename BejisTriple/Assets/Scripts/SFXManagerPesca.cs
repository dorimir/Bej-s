using UnityEngine;

public class SFXManagerPesca : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip enganchar, sacarDelAgua, burbujas, barboWhoosh, fin;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
