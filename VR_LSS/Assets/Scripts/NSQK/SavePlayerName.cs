using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePlayerName : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text nameText;
    public Canvas targetCanvas;

    public void SaveName()
    {
        string playerName = nameInputField.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        nameText.gameObject.SetActive(true);
        nameText.text = $"Oh Shhh, here we go again... {playerName}";

        Debug.Log("Nombre guardado: " + playerName);

        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(true);
        }
    }
}
