using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomerBehavior : MonoBehaviour
{
    public GameObject[] itemIcons; // Array de prefabs de íconos de los objetos
    public Transform iconPosition; // Posición al lado del cliente para mostrar el ícono
    public Transform receivePoint; // Punto donde se posiciona el objeto recibido
    private CustomerSpawner customerSpawner; // Referencia al spawner de clientes

    private GameObject currentIcon; // Referencia al ícono mostrado
    private int requestedItemIndex; // Índice del ítem solicitado
    private bool isSatisfied = false; // Estado del cliente

    [SerializeField] private int currentMoney = 0;

    // Evento que notifica cambios en el dinero
    public UnityEvent<int> OnMoneyChanged;

    void Start()
    {
        RequestItem();

        // Inicializar el evento si es nulo
        if (OnMoneyChanged == null)
        {
            OnMoneyChanged = new UnityEvent<int>();
        }

        // Cargar el dinero almacenado en PlayerPrefs
        currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        OnMoneyChanged.Invoke(currentMoney); // Notificar el cambio inicial
    }

    private void RequestItem()
    {
        // Seleccionar un ítem aleatorio
        requestedItemIndex = Random.Range(0, itemIcons.Length);

        Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);

        // Instanciar el ícono del pedido junto al cliente
        currentIcon = Instantiate(itemIcons[requestedItemIndex], iconPosition.position, spawnRotation);

        // Hacer que el ícono sea hijo del cliente para que se mueva con él
        currentIcon.transform.SetParent(transform);
    }

    public int GetRequestedItemIndex()
    {
        return requestedItemIndex; // Devuelve el índice del ítem solicitado
    }

    // Asignar el CustomerSpawner
    public void SetCustomerSpawner(CustomerSpawner spawner)
    {
        customerSpawner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSatisfied) return; // No aceptar más objetos si ya está satisfecho

        DraggableItem item = other.GetComponent<DraggableItem>();
        if (item != null)
        {
            // Verificar si el ID del objeto coincide con el solicitado
            if (item.itemID == requestedItemIndex)
            {
                Debug.Log("¡Objeto correcto recibido!");
                isSatisfied = true;

                // Incrementar dinero y guardar en PlayerPrefs
                currentMoney += 25;
                PlayerPrefs.SetInt("PlayerMoney", currentMoney);
                Debug.Log("Dinero actual: " + currentMoney);

                // Mover el objeto al punto de recepción del cliente
                other.transform.position = receivePoint.position;

                // Desactivar el ícono del pedido
                Destroy(currentIcon);

                // Desactivar el objeto recibido
                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<DraggableItem>().enabled = false;

                // Notificar al spawner de que este cliente ha recibido su orden
                customerSpawner.OnCustomerDestroyed(this);

                // Destruir el cliente
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("¡Objeto incorrecto!");

                // Reducir dinero y guardar en PlayerPrefs
                currentMoney -= 11; // 11 pesos porque es a lo que sale el qrobus
                PlayerPrefs.SetInt("PlayerMoney", currentMoney);
                Debug.Log("Dinero actual: " + currentMoney);

                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<DraggableItem>().enabled = false;
            }

            // Notificar cambios en el dinero
            OnMoneyChanged.Invoke(currentMoney);
        }
    }

    // Como vimos en POO, si una variable es privada, para acceder a ella se usa un getter
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}
