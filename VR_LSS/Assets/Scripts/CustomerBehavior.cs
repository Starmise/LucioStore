using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] private int currentMoney = 0;

    // Evento que notifica cambios en el dinero
    public UnityEvent<int> OnMoneyChanged;

    private bool isThief = false; // Indica si este cliente es un ladr�n

    public static System.Action<CustomerBehavior> OnCustomerSpawned;
    public static System.Action<string> OnThiefAlert;

    void Start()
    {
        // Verificar si este cliente es un ladr�n basado en su tag
        if (CompareTag("Thief"))
        {
            isThief = true;
            Debug.Log("�Ladr�n detectado!");
            OnThiefAlert?.Invoke("�Cuidado! Un ladr�n apareci� en la escena.");
            HandleThiefBehavior(); // L�gica especial para el ladr�n
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

        // Notificar que esta instancia est� lista
        OnCustomerSpawned?.Invoke(this);

        // Cargar el dinero almacenado en PlayerPrefs
        currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        OnMoneyChanged.Invoke(currentMoney); // Notificar el cambio inicial
    }

    private void RequestItem()
    {
        // Seleccionar un �tem aleatorio
        requestedItemIndex = Random.Range(0, itemIcons.Length);

        Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);

        // Instanciar el �cono del pedido junto al cliente
        currentIcon = Instantiate(itemIcons[requestedItemIndex], iconPosition.position, spawnRotation);

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

                int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
                currentMoney = savedMoney + 25;
                PlayerPrefs.SetInt("PlayerMoney", currentMoney);
                Debug.Log("Dinero actual: " + currentMoney);

                // Mover el objeto al punto de recepci�n del cliente
                other.transform.position = receivePoint.position;

                // Desactivar el �cono del pedido
                Destroy(currentIcon);

                // Desactivar el objeto recibido
                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<DraggableItem>().enabled = false;

                // Notificar al spawner de que este cliente ha recibido su orden
                customerSpawner.OnCustomerDestroyed(this);
            }
            else
            {
                Debug.Log("�Objeto incorrecto!");

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
        Debug.Log("�El ladr�n ha aparecido!");

        // El ladr�n no pide objetos, pero muestra un �cono especial
        Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);

        // Asignar un �cono especial para el ladr�n (DEBE SER EL �LTIMO OBJETO EN itemIcons)
        currentIcon = Instantiate(itemIcons[itemIcons.Length - 1], iconPosition.position, spawnRotation);
        currentIcon.transform.SetParent(transform);

        // Iniciar el temporizador de 10 segundos
        StartCoroutine(ThiefTimer());
    }

    private IEnumerator ThiefTimer()
    {
        Debug.Log("El ladr�n tiene 10 segundos para ser eliminado...");
        yield return new WaitForSeconds(10);

        // Si el ladr�n no fue eliminado en ese tiempo
        if (!isSatisfied)
        {
            Debug.Log("�El ladr�n rob� todo el dinero!");

            // Resetear el dinero del jugador
            currentMoney = 0;
            PlayerPrefs.SetInt("PlayerMoney", currentMoney);
            PlayerPrefs.Save();
            OnMoneyChanged.Invoke(currentMoney);
            OnThiefAlert?.Invoke("�El ladr�n se rob� todo el dinero!");

            // Destruir el ladr�n
            Destroy(gameObject);
        }
    }

    // Detectar colisiones con balas
    private void OnCollisionEnter(Collision collision)
    {
        if (isThief && collision.gameObject.CompareTag("Pellet"))
        {
            Debug.Log("�Ladr�n eliminado por disparo!");

            int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
            currentMoney = savedMoney + 50;
            PlayerPrefs.SetInt("PlayerMoney", currentMoney);
            PlayerPrefs.Save();
            OnMoneyChanged.Invoke(currentMoney);

            Debug.Log("Recompensa recibida: +50. Dinero actual: " + currentMoney);
            OnThiefAlert?.Invoke("�Ladr�n smasheado con �xito!");

            // Destruir el ladr�n
            customerSpawner.OnCustomerDestroyed(this);
            Destroy(gameObject);
        }
    }
}
