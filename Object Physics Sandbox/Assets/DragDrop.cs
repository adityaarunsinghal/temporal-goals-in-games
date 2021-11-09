using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public Rigidbody2D selectedObject;
    Vector3 offset;
    Vector3 mousePosition;
    Vector3 targetPosition;
    public Collider targetObject;

    void Update()
    {
        selectedObject = GetComponent<Rigidbody2D>();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedObject.velocity = Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            // targetObject = Physics2D.OverlapPoint(mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                   targetObject = hit.collider;
                }

            if (targetObject)
            {
                if (selectedObject == targetObject.transform.GetComponent<Rigidbody2D>())
                {
                    targetPosition = mousePosition;
                }
            }
        }
    }

    void LateUpdate()
    {
        selectedObject.MovePosition(targetPosition);
        // selectedObject.transform.position = targetPosition;
    }
}
