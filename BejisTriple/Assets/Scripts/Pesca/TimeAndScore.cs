using UnityEngine;
using TMPro;

public class TimeAndScore : MonoBehaviour
{
    public TextMeshProUGUI timeText, scoreText;

    int score = 0;
    float seconds = 60.0f;
    public GameObject fishSpawner;
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Puntuacion: " + score;
    }

    void Update()
    {
        if (fishSpawner.GetComponent<FishSpawner>().IsGameOn == true)
        {
            seconds -= Time.deltaTime;
            timeText.text = "Tiempo: " + Mathf.FloorToInt(seconds % 60);
        }
        if (seconds <= 0)
        {
            fishSpawner.GetComponent<FishSpawner>().EndGame();
            timeText.text = "¡Fin!";
        } 
    }
}
