using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NSQKCanvas : MonoBehaviour
{
    public InputActionAsset inputActions;

    private Canvas canvas;
    private InputAction menu;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        menu.Enable();
        menu.performed += OpenMenu;
    }

    private void OnDestroy()
    {
        menu.performed -= OpenMenu;
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        canvas.enabled = !canvas.enabled;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("NSQK game");
    }
}