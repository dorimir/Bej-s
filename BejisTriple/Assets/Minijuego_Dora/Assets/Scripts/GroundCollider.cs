using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundCollider : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string groundTag = "suelo";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && this.CompareTag(groundTag))
        {

            if (ContadorDistancia.loseCount >= 3)
            {
                // NO llamar a ResetGame() aquí - se hace desde WinScreenManager
                SceneManager.LoadScene(2);
            }
        }
    }
}