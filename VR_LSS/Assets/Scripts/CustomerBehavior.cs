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

    private bool isThief = false; // Indica si este cliente es un ladrón

    public static System.Action<CustomerBehavior> OnCustomerSpawned;
    public static System.Action<string> OnThiefAlert;

    void Start()
    {
        // Verificar si este cliente es un ladrón basado en su tag
        if (CompareTag("Thief"))
        {
            isThief = true;
            Debug.Log("¡Ladrón detectado!");
            OnThiefAlert?.Invoke("¡Cuidado! Un ladrón apareció en la escena.");
            HandleThiefBehavior(); // Lógica especial para el ladrón
        }
        else
        {
            RequestItem();
        }

        // Inicializar el evento si es nulo
        if (OnMoneyChanged == null)
        {
            OnMoneyChanged = new UnityEvent<int>();
        }

        // Notificar que esta instancia está lista
        OnCustomerSpawned?.Invoke(this);

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

                int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
                currentMoney = savedMoney + 25;
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
            }
            else
            {
                Debug.Log("¡Objeto incorrecto!");

                int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
                currentMoney = savedMoney - 11;// 11 pesos porque es a lo que sale el qrobus
                PlayerPrefs.SetInt("PlayerMoney", currentMoney);
                Debug.Log("Dinero actual: " + currentMoney);

                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<DraggableItem>().enabled = false;
            }
            OnMoneyChanged.Invoke(currentMoney);

            Destroy(gameObject);
        }
    }

    // Como vimos en POO, si una variable es privada, para acceder a ella se usa un getter
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    private void HandleThiefBehavior()
    {
        Debug.Log("¡El ladrón ha aparecido!");

        // El ladrón no pide objetos, pero muestra un ícono especial
        Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);

        // Asignar un ícono especial para el ladrón (DEBE SER EL ÚLTIMO OBJETO EN itemIcons)
        currentIcon = Instantiate(itemIcons[itemIcons.Length - 1], iconPosition.position, spawnRotation);
        currentIcon.transform.SetParent(transform);

        // Iniciar el temporizador de 10 segundos
        StartCoroutine(ThiefTimer());
    }

    private IEnumerator ThiefTimer()
    {
        Debug.Log("El ladrón tiene 10 segundos para ser eliminado...");
        yield return new WaitForSeconds(10);

        // Si el ladrón no fue eliminado en ese tiempo
        if (!isSatisfied)
        {
            Debug.Log("¡El ladrón robó todo el dinero!");

            // Resetear el dinero del jugador
            currentMoney = 0;
            PlayerPrefs.SetInt("PlayerMoney", currentMoney);
            PlayerPrefs.Save();
            OnMoneyChanged.Invoke(currentMoney);
            OnThiefAlert?.Invoke("¡El ladrón se robó todo el dinero!");

            // Destruir el ladrón
            Destroy(gameObject);
        }
    }

    // Detectar colisiones con balas
    private void OnCollisionEnter(Collision collision)
    {
        if (isThief && collision.gameObject.CompareTag("Pellet"))
        {
            Debug.Log("¡Ladrón eliminado por disparo!");

            int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
            currentMoney = savedMoney + 50;
            PlayerPrefs.SetInt("PlayerMoney", currentMoney);
            PlayerPrefs.Save();
            OnMoneyChanged.Invoke(currentMoney);

            Debug.Log("Recompensa recibida: +50. Dinero actual: " + currentMoney);
            OnThiefAlert?.Invoke("¡Ladrón smasheado con éxito!");

            // Destruir el ladrón
            customerSpawner.OnCustomerDestroyed(this);
            Destroy(gameObject);
        }
    }
}
