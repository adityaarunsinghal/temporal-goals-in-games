using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeButton : MonoBehaviour
{
    public float discoloration = 1f;

    public void onButtonPress()
    {
        if (DragDrop3.LastTouchedObject && DragDrop3.LastTouchedObject.GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezeAll)
        {
            DragDrop3.LastTouchedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            DragDrop3.LastTouchedObject.tag = "frozen";
            DragDrop3.LastTouchedObject.GetComponent<SpriteRenderer>().color += new Color (0, 0, 0, discoloration);
        }
        else
        {
            DragDrop3.LastTouchedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            DragDrop3.LastTouchedObject.tag = "notFrozen";
            DragDrop3.LastTouchedObject.GetComponent<SpriteRenderer>().color -= new Color (0, 0, 0, discoloration);
        }
    }
}

