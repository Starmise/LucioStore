using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristUI : MonoBehaviour
{
    public InputActionAsset inputActions;

    private Canvas _wristUICanvas;
    private InputAction _menuAction;

    private CustomerBehavior _customerBehavior; // Accedemos al script con el dinero
    public TMP_Text moneyTxt;

    private void Start()
    {
        _wristUICanvas = GetComponent<Canvas>();
        _menuAction = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        if (_menuAction == null)
        {
            Debug.Log("No hay un mapa de acciones");
        }
        _menuAction.Enable();
        _menuAction.performed += ToggleMenu;

        _customerBehavior = FindObjectOfType<CustomerBehavior>();

        if (_customerBehavior != null)
        {
            // Suscribirse al evento
            _customerBehavior.OnMoneyChanged.AddListener(UpdateMoney);

            // Actualizar el texto inicial
            UpdateMoney(_customerBehavior.GetCurrentMoney());
        }
        else
        {
            Debug.LogError("No está el script del dinero!");
        }
    }

    private void UpdateMoney(int money)
    {
        if (moneyTxt != null)
        {
            moneyTxt.text = "Money: " + money;
        }
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