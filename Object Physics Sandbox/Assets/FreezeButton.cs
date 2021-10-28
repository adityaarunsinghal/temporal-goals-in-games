using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeButton : MonoBehaviour
{
    public static float discoloration = 0.5f;

    public void onButtonPress()
    {
        GameObject lastTouchedObject = DragDrop3.LastTouchedObject;

        if (lastTouchedObject && lastTouchedObject.GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezeAll)
        {
            freeze(lastTouchedObject);
        }
        else
        {
            unfreeze(lastTouchedObject);
        }
    }

    public void freeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        obj.tag = "frozen"; // TODO: make `isFrozen` a property of the gameObject itself?
        obj.GetComponent<SpriteRenderer>().material.color -= new Color(0, 0, 0, discoloration);
    }

    public void unfreeze(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        obj.tag = "notFrozen";
        obj.GetComponent<SpriteRenderer>().material.color += new Color(0, 0, 0, discoloration);
    }
}

