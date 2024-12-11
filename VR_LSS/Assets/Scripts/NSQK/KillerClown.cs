using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerClown : MonoBehaviour
{
    private const int PointsPerKill = 50;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Asegúrate de que el sonido está en loop y se reproduce al inicio.
        audioSource.loop = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El jugador fue alcanzado! Fin del juego.");

            SceneManager.LoadScene("EndNSQK");
        }

        if (other.CompareTag("Pellet"))
        {
            // Suma los puntos al jugador
            AddPoints(PointsPerKill);

            // Destruye al enemigo y al pellet
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    private void AddPoints(int points)
    {
        int currentPoints = PlayerPrefs.GetInt("PlayerScore", 0);
        currentPoints += points;

        PlayerPrefs.SetInt("PlayerScore", currentPoints);
        PlayerPrefs.Save();
    }
}
