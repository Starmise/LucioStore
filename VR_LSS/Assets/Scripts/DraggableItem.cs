using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public int itemID; // ID �nico para cada tipo de objeto

    private Vector3 initialPosition;
    private bool isDragging = false;

    void Start()
    {
        initialPosition = transform.position; // Guardar la posici�n inicial
    }

    void Update()
    {
        if (isDragging)
        {
            // Mover el objeto seg�n la posici�n del mouse en 3D
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                transform.position = hit.point;
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
