using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateButton : MonoBehaviour
{
    Vector3 mousePosition;
    public static bool toggled = false;
    public GameObject selectedObject;
    public Collider2D targetObject;

    public void onButtonPress()
    {
        toggled = !toggled;
        if (toggled)
        {
            RemoveButton.toggled = false;
            FreezeButton2.toggled = false;
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
                        GameObject duplicate = GameObject.Instantiate(selectedObject);
                        duplicate.transform.position = Vector3.zero;
                        if (duplicate.GetComponent<customProperties>().isFrozen)
                        {
                            FreezeButton2.unfreeze(duplicate);
                        }
                        objectsCacher.cacheObject(duplicate);
                    }
                }

            }
        }
    }
}
