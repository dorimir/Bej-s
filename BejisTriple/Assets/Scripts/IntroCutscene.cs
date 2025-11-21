using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    public TextMeshProUGUI textoIntro;
    public int cutsceneCount = 0;
    public GameObject[] siluetas;
    public GameObject cartel, pueblo, mountains, nubes;
    bool bejis = false;

    void Start()
    {
        siluetas[0].GetComponent<Animator>().SetBool("Habla1", true);
        siluetas[1].GetComponent<Animator>().SetBool("Habla2", true);
        textoIntro.text = "Nos encontramos en 1225,\ncuando musulmanes y cristianos\nluchaban por el poder en la península ibérica.";
        StartCoroutine(ChangeStage());
    }
    void Update()
    {
        if(bejis == true)
        {
            cartel.transform.position = Vector3.MoveTowards(cartel.transform.position, new Vector3(1.2f, -2.6f, 13), 6.8f*Time.deltaTime);
            pueblo.transform.position = Vector3.MoveTowards(pueblo.transform.position, new Vector3(1.8f, -1, 14), 7*Time.deltaTime);
            mountains.transform.position = Vector3.MoveTowards(mountains.transform.position, new Vector3(5.6f, 0.5f, 16), 6.5f*Time.deltaTime);
            nubes.transform.position = Vector3.MoveTowards(nubes.transform.position, new Vector3(5.6f, 0.63f, 17), 7*Time.deltaTime);
        }
    }

    private IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(7f);
        cutsceneCount++;
        switch(cutsceneCount)
            {
                case 1:
                textoIntro.text = "Se había producido una disputa,\nentre el príncipe y el califa de Almohade,\nporque el rey Fernando III de Castilla\nhabía nombrado vasallo al príncipe.";
                StartCoroutine(ChangeStage());
                break;
                    case 2:
                    siluetas[0].GetComponent<Animator>().SetBool("Habla1", false);
                    siluetas[1].GetComponent<Animator>().SetBool("Habla2", false);
                    siluetas[2].GetComponent<Animator>().SetBool("Habla1", true);
                    bejis = true;
                textoIntro.text = "Aprovechando la oportunidad,\nel noble Gil Garcés I de Azagra\nconquista Bejís para la cristiandad...";
                        StartCoroutine(ChangeStage());
                break;
                    case 3:
                    siluetas[2].GetComponent<Animator>().SetBool("Habla1", false);
                    siluetas[3].GetComponent<Animator>().SetBool("Habla1", true);
                    siluetas[4].GetComponent<Animator>().SetBool("Habla2", true);
                textoIntro.text = "...solo para que el príncipe almohade\nla recupere en un plan\nelaborado con cuidado.";
                        StartCoroutine(ChangeStage());
                break;
                    case 4:
                    siluetas[3].GetComponent<Animator>().SetBool("Habla1", false);
                    siluetas[4].GetComponent<Animator>().SetBool("Habla2", false);
                    siluetas[5].GetComponent<Animator>().SetBool("Habla2", true);
                textoIntro.text = "Si bien no tenemos muy claro cómo,\nen 1229 Bejís vuelve a ser conquistada\npor los cristianos, esta vez por\ndon Pedro Fernández de Azagra,\nvasallo del rey Jaime I.";
                        StartCoroutine(ChangeStage());
                break;
                    case 5:
                    siluetas[5].GetComponent<Animator>().SetBool("Habla2", false);
                    siluetas[6].GetComponent<Animator>().SetBool("Habla1", true);
                    siluetas[7].GetComponent<Animator>().SetBool("Habla2", true);
                textoIntro.text = "En 1235, Jaime I cede Bejís\na la Orden militar de los Calatrava,\nya que por su estratégica posición\ngeográfica resultaba un puesto muy\nvalioso en la conquista de Valencia\ny así la protegía.";
                        StartCoroutine(ChangeStage());
                break;
                    case 6:
                    siluetas[6].GetComponent<Animator>().SetBool("Habla1", false);
                    siluetas[7].GetComponent<Animator>().SetBool("Habla2", false);
                    siluetas[8].GetComponent<Animator>().SetBool("Habla2", true);
                textoIntro.text = "Bajo el hierro del casco de uno de sus caballeros,\nse esconde un hombre. Acerquémonos a uno\nde los ladrillos del castillo cristiano\ny cómo se prepara para la toma de Valencia.";
                        StartCoroutine(ChangeStage());
                    break;
                default:
                SceneManager.LoadScene("Intro_Cutscene", LoadSceneMode.Single);
                //Lo cambiaremos despues
                        break;
            }
    }
}
