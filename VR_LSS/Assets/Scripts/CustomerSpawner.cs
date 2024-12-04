using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Prefab del cliente
    public Transform spawnPoint; // Punto de spawn inicial
    public float spawnInterval = 5f; // Intervalo de spawn en segundos
    public int maxQueueSize = 3; // Máximo número de clientes en la fila
    public Vector3 queueOffset = new Vector3(0, 0, 2f); // Distancia entre los clientes en la fila

    private Queue<GameObject> customerQueue = new Queue<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (customerQueue.Count < maxQueueSize)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        // Calcular posición del nuevo cliente
        Vector3 spawnPosition = spawnPoint.position + queueOffset * customerQueue.Count;

        // Crear cliente y añadirlo a la fila
        GameObject newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
        CustomerBehavior customerBehavior = newCustomer.GetComponent<CustomerBehavior>();

        // Asignar la referencia al CustomerSpawner
        customerBehavior.SetCustomerSpawner(this);

        customerQueue.Enqueue(newCustomer);

        // Si la fila supera el tamaño máximo, eliminar el cliente más antiguo
        if (customerQueue.Count > maxQueueSize)
        {
            GameObject oldCustomer = customerQueue.Dequeue();
            Destroy(oldCustomer);
            // Llamamos a MoveCustomersForward para que los clientes restantes se muevan a la posición correcta
            MoveCustomersForward();
        }
    }

    // Método llamado cuando un cliente ha sido destruido
    public void OnCustomerDestroyed()
    {
        // Mover a los clientes restantes en la cola hacia adelante
        MoveCustomersForward();
    }

    // Mover los clientes restantes hacia adelante
    private void MoveCustomersForward()
    {
        // Solo mover clientes si hay al menos uno
        if (customerQueue.Count == 0) return;

        // Recorre la cola y mueve los clientes hacia la posición del anterior
        for (int i = 0; i < customerQueue.Count; i++)
        {
            GameObject customer = customerQueue.ToArray()[i];
            Vector3 targetPosition = spawnPoint.position + queueOffset * i; // Nueva posición para el cliente

            // Mueve el cliente suavemente a la nueva posición
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPosition, Time.deltaTime * 2);
        }
    }
}
