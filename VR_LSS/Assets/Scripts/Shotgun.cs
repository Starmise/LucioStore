using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public Transform barrelEnd;
    public GameObject pelletPrefab;
    public float pelletSpeed = 20f;
    public float damage = 10f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private XRGrabInteractable grabInteractable;

    public Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(Fire);
    }

    void Fire(ActivateEventArgs args)
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            GameObject pellet = Instantiate(pelletPrefab, barrelEnd.position, spawnRotation);
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            rb.velocity = barrelEnd.forward * pelletSpeed;

            // Ignorar colisión entre la bala y la escopeta
            Collider shotgunCollider = GetComponent<Collider>();
            Collider pelletCollider = pellet.GetComponent<Collider>();
            if (shotgunCollider != null && pelletCollider != null)
            {
                Physics.IgnoreCollision(shotgunCollider, pelletCollider);
            }
        }
    }
}
