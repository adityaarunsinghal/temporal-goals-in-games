using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeButton2 : MonoBehaviour
{
    Vector3 mousePosition;
    public GameObject selectedObject;
    public static float discoloration = 0.5f;
    public bool toggled = false;

    public void onButtonPress()
    {
        toggled = !toggled;
    }

    void update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (toggled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    selectedObject = targetObject.transform.gameObject;
                    if (selectedObject.GetComponent<Rigidbody2D>())
                    {
                        if (selectedObject.GetComponent<customProperties>().isFrozen)
                        {
                            unfreeze(selectedObject.gameObject);
                        }
                        else
                        {
                            freeze(selectedObject.gameObject);
                        }
                    }
                }
            }

        }
    }

    public void freeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        obj.GetComponent<customProperties>().isFrozen = true; 
        obj.GetComponent<SpriteRenderer>().material.color -= new Color(0.1f, 0.1f, 0.1f, discoloration);
    }

    public void unfreeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        obj.GetComponent<customProperties>().isFrozen = false; 
        obj.GetComponent<SpriteRenderer>().material.color += new Color(0.1f, 0.1f, 0.1f, discoloration);
    }
}

