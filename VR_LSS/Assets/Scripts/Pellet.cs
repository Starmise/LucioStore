using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision)
    {
        // Implementar lógica de daño aquí
        Debug.Log("Hit: " + collision.gameObject.name);
        // Ejemplo: si el objeto tiene un componente de salud, reducir su salud
        // var health = collision.gameObject.GetComponent<Health>();
        // if (health != null)
        // {
        //     health.TakeDamage(damage);
        // }

        Destroy(gameObject); // Destruye el proyectil al impactar
    }
}
