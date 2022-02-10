using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour // classic free moving of objects
{
    public Rigidbody2D selectedObject;
    Vector3 mousePosition;

    void Update()
    {
        // track mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Select object 
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            // if something was actually clicked
            if (targetObject)
            {
                if (targetObject.gameObject.tag != "ball")
                {
                    selectedObject = targetObject.gameObject.GetComponent<Rigidbody2D>();
                }
            }
        }
    }

    // void OnMouseDown()
    // {
    //     selectedObject = GetComponent<Rigidbody2D>();
    // }

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
