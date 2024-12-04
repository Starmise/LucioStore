using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehavior : MonoBehaviour
{
    public GameObject[] itemIcons; // Array de prefabs de �conos de los objetos
    public Transform iconPosition; // Posici�n al lado del cliente para mostrar el �cono
    public Transform receivePoint; // Punto donde se posiciona el objeto recibido
    private CustomerSpawner customerSpawner; // Referencia al spawner de clientes

    private GameObject currentIcon; // Referencia al �cono mostrado
    private int requestedItemIndex; // �ndice del �tem solicitado
    private bool isSatisfied = false; // Estado del cliente

    void Start()
    {
        RequestItem();
    }

    private void RequestItem()
    {
        // Seleccionar un �tem aleatorio
        requestedItemIndex = Random.Range(0, itemIcons.Length);

        // Instanciar el �cono del pedido junto al cliente
        currentIcon = Instantiate(itemIcons[requestedItemIndex], iconPosition.position, Quaternion.identity);

        // Hacer que el �cono sea hijo del cliente para que se mueva con �l
        currentIcon.transform.SetParent(transform);
    }

    public int GetRequestedItemIndex()
    {
        return requestedItemIndex; // Devuelve el �ndice del �tem solicitado
    }

    // Asignar el CustomerSpawner
    public void SetCustomerSpawner(CustomerSpawner spawner)
    {
        customerSpawner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSatisfied) return; // No aceptar m�s objetos si ya est� satisfecho

        DraggableItem item = other.GetComponent<DraggableItem>();
        if (item != null)
        {
            // Verificar si el ID del objeto coincide con el solicitado
            if (item.itemID == requestedItemIndex)
            {
                Debug.Log("�Objeto correcto recibido!");
                isSatisfied = true;

                // Mover el objeto al punto de recepci�n del cliente
                other.transform.position = receivePoint.position;

                // Desactivar el �cono del pedido
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
                Debug.Log("�Objeto incorrecto!");
            }
        }
    }
}
