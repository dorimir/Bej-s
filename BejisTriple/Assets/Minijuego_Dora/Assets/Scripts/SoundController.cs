using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;       // para efectos
    public AudioSource ambientSource;   // para ambiente (loop)

    [Header("Clips")]
    public AudioClip ambient;
    public AudioClip arrowShot;
    public AudioClip apuntar;
    public AudioClip collisionPotenciador;
    public AudioClip collisionObstaculo;
    public AudioClip collisionGround;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip continueSound;
    public AudioClip retry;

    private void Awake()
    {
        // Singleton básico
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Reproducir ambiente si está asignado
        if (ambient != null && ambientSource != null)
        {
            ambientSource.clip = ambient;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    // -----------------------
    //     MÉTODOS PÚBLICOS
    // -----------------------

    public void PlayArrowShot() => PlaySFX(arrowShot);
    public void PlayApuntar() => PlaySFX(apuntar);
    public void PlayCollisionPotenciador() => PlaySFX(collisionPotenciador);
    public void PlayCollisionObstaculo() => PlaySFX(collisionObstaculo);
    public void PlayCollisionGround() => PlaySFX(collisionGround);
    public void PlayWin() => PlaySFX(win);
    public void PlayLose() => PlaySFX(lose);
    public void PlayContinue() => PlaySFX(continueSound);
    public void PlayRetry() => PlaySFX(retry);

    // Método genérico
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}