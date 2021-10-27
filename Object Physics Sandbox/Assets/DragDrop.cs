using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragDrop : MonoBehaviour
{
    public Rigidbody2D selectedObject;
    Vector3 offset;
    Vector3 mousePosition;

    public float maxSpeed = 20;
    Vector2 mouseForce;
    Vector3 lastPosition;

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (selectedObject)
        {
            mouseForce = (mousePosition - lastPosition) / Time.deltaTime;
            mouseForce = Vector2.ClampMagnitude(mouseForce, maxSpeed);
            lastPosition = mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                offset = selectedObject.transform.position - mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {

            selectedObject.velocity = Vector2.zero;
            selectedObject.AddForce(mouseForce, ForceMode2D.Impulse);
            selectedObject = null;
        }
    }

    void FixedUpdate()
    {
        if (selectedObject)
        {
            selectedObject.MovePosition(mousePosition + offset);
        }
    }
}
