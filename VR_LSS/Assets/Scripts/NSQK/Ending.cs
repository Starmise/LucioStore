using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public TMP_Text pointsText;     //puntos 

    void Start()
    {
        int currentPoints = PlayerPrefs.GetInt("PlayerScore", 0);
        pointsText.text = $"Well, you got: " +
            $"${currentPoints}";
    }

    public void OnRestart()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("NSQK game");
    }
}
