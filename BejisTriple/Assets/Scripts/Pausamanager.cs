using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pausamanager : MonoBehaviour
{
    [SerializeField] private Button pauseButton;   // <-- botón asignable desde inspector

    public GameObject Menupausa;
    public GameObject menuconfirmacion;

    private bool pausado = false;
    private bool pausandoAhora = false;

    void Start()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(() =>
            {
                if (pausado) Despausar();
                else Pausar();
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado) Despausar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        if (pausandoAhora) return;

        Menupausa.SetActive(true);
        pausandoAhora = true;

        Invoke(nameof(ActivarTimeScaleCero), 0.2f);
    }

    public void TogglePause()
    {
        if (pausado) Despausar();
        else Pausar();
    }

    private void ActivarTimeScaleCero()
    {
        Time.timeScale = 0f;
        pausado = true;
        pausandoAhora = false;
    }

    public void Despausar()
    {
        Time.timeScale = 1f;
        Menupausa.SetActive(false);
        pausado = false;
    }

    public void Salir()
    {
        Menupausa.SetActive(false);
        menuconfirmacion.SetActive(true);
    }

    public void confirmarSalir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void NoSalirMenu()
    {
        menuconfirmacion.SetActive(false);
        Menupausa.SetActive(true);
    }

    public void continuar()
    {
        Despausar();
    }
}
