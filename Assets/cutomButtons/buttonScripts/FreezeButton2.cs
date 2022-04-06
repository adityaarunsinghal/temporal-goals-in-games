using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeButton2 : MonoBehaviour
{
    Vector3 mousePosition;
    public GameObject selectedObject;
    public static float discoloration = 0.5f;
    public static bool toggled = false;
    public Collider2D targetObject;

    public void onButtonPress()
    {
        toggled = !toggled;
        {
            DuplicateButton.toggled = false;
            RemoveButton.toggled = false;
        }
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
                        if (selectedObject.GetComponent<customProperties>().isFrozen)
                        {
                            unfreeze(selectedObject);
                        }
                        else
                        {
                            freeze(selectedObject);
                        }
                    }
                }

            }
        }
    }

    public static void freeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        obj.GetComponent<customProperties>().isFrozen = true;
        obj.GetComponent<SpriteRenderer>().color -= new Color(0.1f, 0.1f, 0.1f, discoloration);
    }

    public static void unfreeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        obj.GetComponent<customProperties>().isFrozen = false;
        obj.GetComponent<SpriteRenderer>().color += new Color(0.1f, 0.1f, 0.1f, discoloration);
    }
}

