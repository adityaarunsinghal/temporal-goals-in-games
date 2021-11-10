using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public Rigidbody2D selectedObject;
    Vector3 mousePosition;

    void OnMouseDown()
    {
        selectedObject = GetComponent<Rigidbody2D>();
    }

    void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedObject.position = mousePosition;
    }
}
