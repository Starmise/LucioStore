using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine.UI;

public class ShowKeyboard : MonoBehaviour
{
    [SerializeField] private Button showKeyboardButton;
    [SerializeField] private TMP_InputField linkedInputField;

    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = -0.5f;

    public Transform positionSource;

    void Start()
    {
        if (showKeyboardButton != null)
        {
            showKeyboardButton.onClick.AddListener(KeyboardDirection);
        }
    }

    public void KeyboardDirection()
    {
        if (NonNativeKeyboard.Instance == null)
        {
            Debug.LogError("NonNativeKeyboard.Instance no está inicializado.");
            return;
        }

        // Configura el input field asociado al teclado
        if (linkedInputField != null)
        {
            NonNativeKeyboard.Instance.InputField = linkedInputField;
            NonNativeKeyboard.Instance.PresentKeyboard(linkedInputField.text); 
        }
        else
        {
            NonNativeKeyboard.Instance.PresentKeyboard();
        }

        // Para poder colocar el teclado siempre en frente del jugador, necesitamos
        // obtener 3 vectores, el vector de la cámara, el vector del jugador al teclado,
        // y el vector del suelo al teclado.

        Vector3 direction = positionSource.forward;
        direction.y = 0; // El teclado siempre debe estar horizontal
        direction.Normalize();

        // La posición del teclado será el vector de la cámara mas la dirección a la que ve
        // el jugador multiplicado por la distancia, más el vector de la altura.
        Vector3 targetPos = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        
        // Accedemos a nuestro asset de teclado y lo reposicionamos
        NonNativeKeyboard.Instance.RepositionKeyboard(targetPos);

        NonNativeKeyboard.Instance.PresentKeyboard();
    }
}
