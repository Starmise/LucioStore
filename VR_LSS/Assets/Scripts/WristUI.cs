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

    // private CustomerBehavior _customerBehavior; // Accedemos al script con el dinero
    public TMP_Text moneyTxt;
    public TMP_Text alertTxt;

    private void Awake()
    {
        // Suscribirse a los eventos globales quetengamos
        CustomerBehavior.OnCustomerSpawned += InitializeCustomerBehavior;
        CustomerBehavior.OnThiefAlert += UpdateAlertMessage;
    }

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

        // Mostrar un valor inicial basado en PlayerPrefs
        UpdateMoney(PlayerPrefs.GetInt("PlayerMoney", 0));
        UpdateAlertMessage("Todo bien por el momento, sigue así Lucio.");
    }

    private void UpdateMoney(int money)
    {
        if (moneyTxt != null)
        {
            moneyTxt.text = "Money: " + money;
        }
    }

    private void UpdateAlertMessage(string message)
    {
        if (alertTxt != null)
        {
            alertTxt.text = message;
        }
    }

    private void OnDestroy()
    {
        _menuAction.performed -= ToggleMenu;

        // Desuscribirse del evento global para evitar errores
        CustomerBehavior.OnCustomerSpawned -= InitializeCustomerBehavior;
    }

    private void InitializeCustomerBehavior(CustomerBehavior customerBehavior)
    {
        // Suscribirse al evento local del cliente
        customerBehavior.OnMoneyChanged.AddListener(UpdateMoney);

        // Actualizar la interfaz con el dinero actual del cliente
        UpdateMoney(customerBehavior.GetCurrentMoney());
        // Debug.Log("WristUI ahora conectado al nuevo CustomerBehavior.");
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("El botón de Menu fue seleccionado");

        _wristUICanvas.enabled = !_wristUICanvas.enabled;
    }
}