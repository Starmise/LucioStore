using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    public TMP_Text moneyText; // Referencia al texto para mostrar el dinero

    void Start()
    {
        // Obtener el dinero guardado en PlayerPrefs y mostrarlo
        int finalMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        moneyText.text = "Dinero Final: " + finalMoney;
    }

    // Método para regresar al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena de menú principal
    }
}
