using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public Rigidbody2D selectedObject;
    Vector3 offset;
    Vector3 mousePosition;
    Vector3 targetPosition;

    void Update()
    {
        selectedObject = GetComponent<Rigidbody2D>();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject==targetObject.transform.GetComponent<Rigidbody2D>())
            {
                targetPosition = selectedObject.transform.position;
            }
        }

        selectedObject.MovePosition(targetPosition);
    }
}
