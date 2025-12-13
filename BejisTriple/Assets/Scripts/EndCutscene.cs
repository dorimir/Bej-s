using UnityEngine;
using System.Collections;

public class EndCutscene : MonoBehaviour
{
    public Animator pag1, pag2, pag3;
    public int seconds;
    bool isonPage2 = false, isonPage3=false;

    void Awake()
    {
        pag1.SetTrigger("Aclarar");
        StartCoroutine(PasarPagina());
    }

    private IEnumerator PasarPagina()
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("pasar pagina");
        if(!isonPage3)
        {
            NextPage();
            StartCoroutine(PasarPagina());
        }
    }

    public void NextPage()
    {
        if(!isonPage2)
        {
            pag1.SetTrigger("Desvanecer");
            pag2.SetTrigger("Aclarar");
            isonPage2=true;
        }
        else
        {
            pag2.SetTrigger("Desvanecer");
            pag3.SetTrigger("Aclarar");
            isonPage3 = true;
        }
    }
}
