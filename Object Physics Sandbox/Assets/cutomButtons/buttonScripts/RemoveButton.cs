using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButton : MonoBehaviour
{
    Vector3 mousePosition;
    public static bool toggled = false;
    public GameObject selectedObject;
    public Collider2D targetObject;

    public void onButtonPress()
    {
        toggled = !toggled;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (toggled)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    selectedObject = targetObject.transform.gameObject;
                    if (selectedObject.GetComponent<Rigidbody2D>())
                    {
                        Destroy(selectedObject);
                    }
                }

            }
        }
    }
}
