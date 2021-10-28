using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop3 : MonoBehaviour
{
    public Rigidbody2D selectedObject;
    public static GameObject LastTouchedObject;
    Vector3 mousePosition;
    public float maxSpeed = 20;
    public float dragSpeed = 8;
    public float throwSpeed = 10;
    Vector2 mouseForce;
    Vector3 lastPosition;

    void Update()
    {
        // track mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Select object 
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                if (selectedObject)
                {
                    LastTouchedObject = targetObject.transform.gameObject;
                }
            }
        }

        // move selected object
        if (selectedObject)
        {
            Vector2 dir = mousePosition - selectedObject.transform.position;
            dir *= dragSpeed;
            selectedObject.velocity = dir;
        }

        // find mouseForce
        if (selectedObject)
        {
            mouseForce = throwSpeed * (mousePosition - lastPosition) / (Time.deltaTime * dragSpeed);
            mouseForce = Vector2.ClampMagnitude(mouseForce, maxSpeed);
            lastPosition = mousePosition;
        }

        // throw selected object
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.velocity = Vector2.zero;
            selectedObject.AddForce(mouseForce, ForceMode2D.Impulse);
            selectedObject = null;
        }

    }

    public void addDragDrop1()
    {
        
    }
}
