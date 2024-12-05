using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Prefab del cliente
    public Transform spawnPoint; // Punto de spawn inicial
    public float spawnInterval = 5f; // Intervalo de spawn en segundos
    public int maxQueueSize = 3; // Máximo número de clientes en la fila
    public Vector3 queueOffset = new Vector3(0, 0, 2f); // Distancia entre los clientes en la fila

    public GameObject thiefPrefab;
    [SerializeField] private float thiefProbability = 0.05f; // Probabilidad del 5%

    private Queue<GameObject> customerQueue = new Queue<GameObject>();
    private int totalCustomersSpawned = 0;
    [SerializeField] private int maxTotalCustomers = 12; 

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

            // Hay que checar si ya se llegó al lmite LUCIOOOO
            if (totalCustomersSpawned >= maxTotalCustomers)
            {
                Debug.Log("Se alcanzó el límite de clientes. Cargando la escena FinalScene...");
                SceneManager.LoadScene("Final Scene");
                yield break; // Detener la corrutina
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        // Calcular posición del nuevo cliente
        Vector3 spawnPosition = spawnPoint.position + queueOffset * customerQueue.Count;

        // Determinar si se debe instanciar un Thief en lugar de un cliente normal
        GameObject newCustomer;
        if (Random.value <= thiefProbability)
        {
            newCustomer = Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("Thief instanciado!");
        }
        else
        {
            newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("Cliente normal llamado.");
        }

        CustomerBehavior customerBehavior = newCustomer.GetComponent<CustomerBehavior>();

        // Asignar la referencia al CustomerSpawner
        customerBehavior.SetCustomerSpawner(this);

        customerQueue.Enqueue(newCustomer);
        totalCustomersSpawned++;

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
    public void OnCustomerDestroyed(CustomerBehavior customer)
    {
        // Filtrar la cola para remover el cliente destruido
        customerQueue = new Queue<GameObject>(
            customerQueue.Where(c => c != null && c != customer.gameObject)
        );

        // Mover a los clientes restantes en la cola hacia adelante
        MoveCustomersForward();
    }

    // Mover los clientes restantes hacia adelante
    private void MoveCustomersForward()
    {
        // Solo mover clientes si hay al menos uno
        if (customerQueue.Count == 0) return;

        List<GameObject> customerVerify = new List<GameObject>();

        int idCustomers = 0;
        foreach (GameObject customer in customerQueue)
        {
            Vector3 targetPosition = spawnPoint.position + queueOffset * idCustomers; // Nueva posición para el cliente
            // Mueve el cliente suavemente a la nueva posición
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPosition, Time.deltaTime * 2);
            customerVerify.Add(customer);
            idCustomers++;
        }

        customerQueue = new Queue<GameObject>(customerVerify);
    }
}
