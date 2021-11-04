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

    void FixedUpdate()
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
}


// objects shouldnt move when setting up  --> finite state machine 
// freeze and unfreeze should apply to object touched AFTER the button is toggled --> 
// have inventory of objects so people can put it in/out upto a certain number 
// --> show brain it on, other physics game from android