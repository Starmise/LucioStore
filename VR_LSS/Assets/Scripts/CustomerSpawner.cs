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
        customerQueue.Enqueue(newCustomer);

        // Si la fila supera el tamaño máximo, eliminar el cliente más antiguo
        if (customerQueue.Count > maxQueueSize)
        {
            GameObject oldCustomer = customerQueue.Dequeue();
            Destroy(oldCustomer);
        }
    }
}
