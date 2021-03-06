using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour // classic free moving of objects
{
    public Rigidbody2D selectedObject;
    Vector3 mousePosition;

    void OnMouseDown()
    {
        selectedObject = GetComponent<Rigidbody2D>();
    }

    void OnMouseDrag()
    {
        if (selectedObject)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.position = mousePosition;
        }
    }
    void OnMouseUp()
    {
        selectedObject = null;
    }
}
