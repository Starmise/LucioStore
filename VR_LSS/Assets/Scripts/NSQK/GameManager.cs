using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;       //temporizador
    public TMP_Text playerNameText; //nombre del jugador
    public TMP_Text pointsText;     //puntos 

    private float timeRemaining = 120f; // Tiempo 2 minutos
    private bool isTimerRunning = true;

    private const int SurvivalBonus = 100000;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
        playerNameText.text = $"Survivor: {playerName}";
    }

    void Update()
    {
        int currentPoints = PlayerPrefs.GetInt("PlayerScore", 0);
        pointsText.text = $"Points: {currentPoints}";

        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;

                AddSurvivalBonus();

                // Cambia a la escena final
                SceneManager.LoadScene("EndNSQK");
            }
        }
    }

    void UpdateTimerUI()
    {
        // Convierte el tiempo restante en minutos y segundos.
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Actualiza el texto del temporizador.
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void AddSurvivalBonus()
    {
        // Obtén los puntos actuales y suma el bono de supervivencia
        int currentPoints = PlayerPrefs.GetInt("PlayerScore", 0);
        currentPoints += SurvivalBonus;

        // Guarda los puntos actualizados
        PlayerPrefs.SetInt("PlayerScore", currentPoints);
        PlayerPrefs.Save();
    }
}
