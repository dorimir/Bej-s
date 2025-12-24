using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FishSpawner : MonoBehaviour
{
    public GameObject carpa, trucha, siluro, barbo, basura, bubbleParticles;
    public AudioClip musica;
    public TextMeshProUGUI SeAcaboText, RetryAndExitText;
    public GameObject tutorial;
    public AudioSource audioSource, audioSourceMusica;

    public GameObject anzuelo, TimeAndScore;
    public float speed = 3;

    public bool IsGameOn = false;
    public int contadorPeces =0;

    /*
    
    Cada segundo aparece un nuevo pez.
    8/20 veces aparece una trucha (40%)
    8/20 veces aparece una carpa (40%)
    3/20 veces aparece un siluro (15%)
    1/20 veces aparece un barbo dorado (5%)
    
    */

    void Update()
    {
        Debug.Log("contador peces: " + contadorPeces);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        if(IsGameOn == false && TimeAndScore.GetComponent<TimeAndScore>().seconds <=0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SceneManager.LoadScene("Fishing Minigame", LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene("Rio", LoadSceneMode.Single);
            }
        }
    }

    public void StartGame()
    {
        tutorial.SetActive(false);
        IsGameOn = true;
        audioSourceMusica.clip = musica;
        audioSourceMusica.Play();
        StartCoroutine(SpawnFish());
        StartCoroutine(SpawnTrash());
    }

    public void EndGame()
    {
        IsGameOn = false;
        anzuelo.SetActive(false);
        SeAcaboText.gameObject.SetActive(true);
        RetryAndExitText.gameObject.SetActive(true);
        if (TimeAndScore.GetComponent<TimeAndScore>().score >= 40)
        {
            SeAcaboText.text = "¡HAS GANADO!";
            //Hacer las acciones necesarias cuando se gane
            GameManager.Instance.minijuegoCompletado(2);
        }
        else  SeAcaboText.text = "¡QUÉ LÁSTIMA!";
    }

    private IEnumerator SpawnFish()
    {
        yield return new WaitForSeconds(0.6f);
        if(contadorPeces<25)
        {
            contadorPeces++;
        var rand = new System.Random();
        GameObject spawnedFish;

        //Se decide a qué profundidad aparecerá el pez
        float spawnPointY = (float)((rand.NextDouble() * 5.5f) + 6.4f);

        //Se decide si aparecerá a la izquierda o a la derecha
        double Xchooser = rand.NextDouble();
        float spawnPointX;
        if (Xchooser >= 0.5) spawnPointX = 10;
        else spawnPointX = -10;

        //Se decide qué pez será
        double randomNumber = rand.NextDouble();
        if (randomNumber <= 0.05)
        {
            //Spawn barbo dorado

            //Mostrar las partículas de burbujas
            if (spawnPointX < 0)
            {
                GameObject burbujas = Instantiate(bubbleParticles, new UnityEngine.Vector3(-8.5f, spawnPointY, 2), UnityEngine.Quaternion.identity);
                burbujas.transform.Rotate(0, 90, 0);
            }
            else
            {
                Instantiate(bubbleParticles, new UnityEngine.Vector3(8.5f, spawnPointY, 2), UnityEngine.Quaternion.identity);
            }
            audioSource.clip = audioSource.gameObject.GetComponent<SFXManagerPesca>().burbujas;
            audioSource.Play();
            StartCoroutine(SpawnBarbo(spawnPointX, spawnPointY));
            yield break;
        }
        else if (randomNumber <= 0.15)
        {
            //Spawn siluro
            spawnedFish = Instantiate(siluro, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
            speed = 5;
        }
        else if (randomNumber <= 0.40)
        {
            //Spawn carpa
            spawnedFish = Instantiate(carpa, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
            speed = 5;
        }
        else
        {
            //Spawn trucha
            spawnedFish = Instantiate(trucha, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
            speed = 3;
        }

        //Si el pez empieza desde la izquierda, mirará a la derecha
        if (spawnPointX < 0) spawnedFish.GetComponent<Transform>().localScale = new UnityEngine.Vector3(-0.52f, 0.52f, 0.52f);

        //Velocidad del pez
        spawnedFish.GetComponent<Rigidbody>().linearVelocity = new UnityEngine.Vector2(speed * (-spawnPointX / 10), spawnedFish.GetComponent<Rigidbody>().linearVelocity.y);

        StartCoroutine(DeleteFish(spawnedFish, 7f));
        }
        if(IsGameOn) StartCoroutine(SpawnFish());
    }
    
    private IEnumerator SpawnBarbo(float spawnPointX, float spawnPointY)
    {
        yield return new WaitForSeconds(2f);
        audioSource.clip = audioSource.gameObject.GetComponent<SFXManagerPesca>().barboWhoosh;
        audioSource.Play();
        GameObject spawnedFish = Instantiate(barbo, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
        speed = 10;

        //Si el pez empieza desde la izquierda, mirará a la derecha
        if (spawnPointX < 0) spawnedFish.GetComponent<Transform>().localScale = new UnityEngine.Vector3(-0.52f, 0.52f, 0.52f);

        //Velocidad del pez
        spawnedFish.GetComponent<Rigidbody>().linearVelocity = new UnityEngine.Vector2(speed * (-spawnPointX / 10), spawnedFish.GetComponent<Rigidbody>().linearVelocity.y);

        StartCoroutine(DeleteFish(spawnedFish, 7f));
        if(IsGameOn) StartCoroutine(SpawnFish());
    }
    
    private IEnumerator SpawnTrash()
    {
        yield return new WaitForSeconds(0.8f);
        if(contadorPeces<25)
        {
            contadorPeces++;
        var rand = new System.Random();
        //Profundidad a la que aparecerá la basura
        float spawnPointY = (float)((rand.NextDouble() * 5) + 6.4f);
        //Si aparecerá a la izquierda o derecha
        double Xchooser = rand.NextDouble();
        float spawnPointX;
        if (Xchooser >= 0.5) spawnPointX = 10;
        else spawnPointX = -10;
        //Creamos la basura
        GameObject spawnedTrash = Instantiate(basura, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);

        //Decidimos si será una herradura rota, un champiñón o un hueso
        double randomNumber = rand.NextDouble();
        if (randomNumber <= 0.33f)
        {
            spawnedTrash.GetComponent<Animator>().SetBool("champinon", true);
        }
        else if (randomNumber <= 0.67f)
        {
            spawnedTrash.GetComponent<Animator>().SetBool("hueso", true);
        }

        //Velocidad
        spawnedTrash.GetComponent<Rigidbody>().linearVelocity = new UnityEngine.Vector2(3 * (-spawnPointX / 10), spawnedTrash.GetComponent<Rigidbody>().linearVelocity.y);
        
        StartCoroutine(DeleteFish(spawnedTrash, 7f));
        }
        if(IsGameOn) StartCoroutine(SpawnTrash());
    }

    private IEnumerator DeleteFish(GameObject fish, float time)
    {
        yield return new WaitForSeconds(time);
        contadorPeces--;
        if(anzuelo.transform.GetChild(1).gameObject != fish) Destroy(fish);
    }
}
