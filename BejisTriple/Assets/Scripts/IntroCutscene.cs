using UnityEngine;
using TMPro;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    public TextMeshProUGUI textoIntro;
    public int cutsceneCount = 0;

    void Start()
    {
        textoIntro.text = "Nos encontramos en 1225,\ncuando musulmanes y cristianos\nluchaban por el poder en la península ibérica.";
        StartCoroutine(ChangeStage());
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
                textoIntro.text = "Aprovechando la oportunidad,\nel noble Gil Garcés I de Azagra\nconquista Bejís para la cristiandad...";
                        StartCoroutine(ChangeStage());
                break;
                    case 3:
                textoIntro.text = "...solo para que el príncipe almohade\nla recupere en un plan\nelaborado con cuidado.";
                        StartCoroutine(ChangeStage());
                break;
                    case 4:
                textoIntro.text = "Si bien no tenemos muy claro cómo,\nen 1229 Bejís vuelve a ser conquistada\npor los cristianos, esta vez por\ndon Pedro Fernández de Azagra,\nvasallo del rey Jaime I.";
                        StartCoroutine(ChangeStage());
                break;
                    case 5:
                textoIntro.text = "En 1235, Jaime I cede Bejís\na la Orden militar de los Calatrava,\nya que por su estratégica posición\ngeográfica resultaba un puesto muy\nvalioso en la conquista de Valencia\ny así la protegía.";
                        StartCoroutine(ChangeStage());
                break;
                    case 6:
                textoIntro.text = "Bajo el hierro del casco de uno de sus caballeros,\nse esconde un hombre. Acerquémonos a uno\nde los ladrillos del castillo cristiano\ny cómo se prepara para la toma de Valencia.";
                        StartCoroutine(ChangeStage());
                    break;
                default:
                        break;
            }
    }
}
