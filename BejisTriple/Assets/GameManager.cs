using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioSource audioSource;
    public static GameManager Instance { get; private set; }
    int contadorMinijuegos = 0;

    public static string nextSpawnID;

    public static Vector3 originalPlayerScale;

    

    /*
    -----------------------------------------
      NÚMEROS CORRESPONDIENTES A MINIJUEGOS
    -----------------------------------------
    
    0 -> Ningún minijuego completado
    1 -> Minijuego de Dani completado
    2 -> Minijuego de Fer completado (+ los anteriores)
    3 -> Minijuego de Alexfa  completado (+ los anteriores)
    4 -> Minijuego de Dora  completado (+ los anteriores)
    5 -> Minijuego de Raúl  completado (+ los anteriores)

    --------------------------------------------------------------------------

    ¿CÓMO PROGRAMAR QUE SE REGISTRE QUE UN MINIJUEGO SE HA COMPLETADO?

    Como este archivo es un singleton (hay uno y sólo uno, se puede llamar desde cualquier script)
    En el momento en el que se el jugador gane el minijuego, hay que escribir la siguiente línea de código:

    GameManager.Instance.minijuegoCompletado(X);

    donde X es el número que corresponde al minijuego (arriba)

    -------------------------------------------------------------------------

    NOTA IMPORTANTE

    Este objeto se crea en la pantalla de título, si empiezas a jugar desde cualquier otra escena NO SE HABRÁ CREADO
    y no funcionará
    Tienes dos opciones:

    1. Empezar todos los tests desde la pantalla de título (¿qué te pasa en la cabeza?)
    2. Crear un objeto vacío que tenga este script en la escena de tu minijuego. Te servirá para hacer pruebas y,
    como solo puede haber uno, si detecta que se ha creado antes se destruirá

    
    */
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //Se crea el singleton la primera vez
            DontDestroyOnLoad(gameObject); //Se hace que se mantenga entre escenas
            SceneManager.sceneLoaded += OnSceneLoaded;
        }else
        {
            Destroy(gameObject); //Si creamos un segundo Game Manager, este ultimo se destruye
        }
        audioSource = GetComponent<AudioSource>();
        
    }

    public void minijuegoCompletado(int nuevoValor)
    {
        contadorMinijuegos = nuevoValor;
        //También se podría hacer contadorMinijuegos++, pero de esta forma nos aseguramos de que no se incrementaría el contador
        //si se llegara a repetir un minijuego
    }

    public int ContadorDeMinijuegos()
    {
        return contadorMinijuegos;
    }
    public void sonidoCambiarEscena (AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(nextSpawnID))
        {
            SpawnPoint[] spawns = FindObjectsOfType<SpawnPoint>();
            foreach (var sp in spawns)
            {
                if (sp.spawnID == nextSpawnID)
                {
                    
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = sp.transform.position;
                    player.transform.rotation = sp.transform.rotation;
                    
                    break;
                    
                }
            }

            nextSpawnID = null;
        }
    }
}
