using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropLinear : MonoBehaviour // pedestal movement
{
    public Rigidbody2D selectedObject;
    Vector3 mousePosition;
    public float dragSpeed = 8;

    void OnMouseDown()
    {
        selectedObject = GetComponent<Rigidbody2D>();
    }

    void OnMouseDrag()
    {
        if (selectedObject)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float new_x = Mathf.Clamp(mousePosition.x, EnvironmentVariables.minX, EnvironmentVariables.maxX);
            selectedObject.MovePosition(new Vector2(new_x, selectedObject.position.y));

            // Vector2 dir = mousePosition - selectedObject.transform.position;
            // dir *= dragSpeed;
            // selectedObject.velocity = dir;
        }
    }
    void OnMouseUp()
    {
        selectedObject = null;
    }
}
