using UnityEngine;
using UnityEngine.UI;

public class UI_Combate : MonoBehaviour
{
    public AudioClip[] audios;
    public AudioSource audioSource;

    public Button Boton_Ataque;
    public Button Boton_Defensa;
    public Button Boton_Finalizar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Boton_Ataque.OnPointerDown(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
            audioSource.PlayOneShot(audios[0],0.1f);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Boton_Ataque.OnPointerUp(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Boton_Defensa.OnPointerDown(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
            audioSource.PlayOneShot(audios[1],0.1f);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Boton_Defensa.OnPointerUp(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Boton_Finalizar.OnPointerDown(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
            audioSource.PlayOneShot(audios[2],0.1f);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Boton_Finalizar.OnPointerUp(
                new UnityEngine.EventSystems.PointerEventData(
                    UnityEngine.EventSystems.EventSystem.current
                )
            );
        }
    }
}
