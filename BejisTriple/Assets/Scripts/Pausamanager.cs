using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausamanager : MonoBehaviour
{

    public GameObject Menupausa;
    public GameObject menuconfirmacion;
    private bool pausado = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado) despausar();
            else pausa();
        }
    }

    public void pausa(){
        Menupausa.SetActive(true);
        Time.timeScale = 0f;
        pausado=true;
    }

    public void despausar(){
        Menupausa.SetActive(false);
        Time.timeScale = 1f;
        pausado=false;
    }



    public void Salir(){
        Menupausa.SetActive(false);
        menuconfirmacion.SetActive(true);

    }

    public void confirmarSalir(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void NoSalirMenu(){
        menuconfirmacion.SetActive(false);
        Menupausa.SetActive(true);

    }

    public void continuar(){
        despausar();
    }
}
