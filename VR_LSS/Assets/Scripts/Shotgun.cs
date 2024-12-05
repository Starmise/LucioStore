using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public Transform barrelEnd;
    public GameObject pelletPrefab;
    public float pelletSpeed = 30f;
    public float damage = 10f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private XRGrabInteractable grabInteractable;

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
            GameObject pellet = Instantiate(pelletPrefab, barrelEnd.position, barrelEnd.rotation);
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            rb.velocity = barrelEnd.forward * pelletSpeed;
        }
    }
}
