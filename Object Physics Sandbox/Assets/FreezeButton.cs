using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeButton : MonoBehaviour
{
    public void onButtonPress()
    {
        if (DragDrop3.LastTouchedObject && DragDrop3.LastTouchedObject.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            DragDrop3.LastTouchedObject.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (DragDrop3.LastTouchedObject && DragDrop3.LastTouchedObject.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            DragDrop3.LastTouchedObject.constraints = RigidbodyConstraints2D.None;
        }
    }
}

// add constraints to rigidbody in script 
// debug this later

// have a list of all objects and cache that (bunch of vector2s) --> return later when going back to setup phase
// rigidbody use gravity can turn on or off when entering/exiting setup phase
