using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristUI : MonoBehaviour
{
    public InputActionAsset inputActions;

    private Canvas _wristUICanvas;
    private InputAction _menuAction;

    private void Start()
    {
        _wristUICanvas = GetComponent<Canvas>();
        _menuAction = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        if (_menuAction == null )
        {
            Debug.Log("No hay un mapa de acciones");
        }
        _menuAction.Enable();
        _menuAction.performed += ToggleMenu;
    }

    private void OnDestroy()
    {
        _menuAction.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("El botón de Menu fue seleccionado");

        _wristUICanvas.enabled = !_wristUICanvas.enabled;
    }
}
