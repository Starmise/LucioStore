using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        RequestItem();
    }

    private void RequestItem()
    {
        // Seleccionar un ítem aleatorio
        requestedItemIndex = Random.Range(0, itemIcons.Length);

        // Instanciar el ícono del pedido junto al cliente
        currentIcon = Instantiate(itemIcons[requestedItemIndex], iconPosition.position, Quaternion.identity);

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

                // Mover el objeto al punto de recepción del cliente
                other.transform.position = receivePoint.position;

                // Desactivar el ícono del pedido
                Destroy(currentIcon);

                // Desactivar el objeto recibido
                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<DraggableItem>().enabled = false;

                // Notificar al spawner de que este cliente ha recibido su orden
                customerSpawner.OnCustomerDestroyed();

                // Destruir el cliente
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("¡Objeto incorrecto!");
            }
        }
    }
}
