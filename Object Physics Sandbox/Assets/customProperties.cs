using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customProperties : MonoBehaviour
{

    public bool isFrozen = false;

    void OnBecameInvisible()
    {
        objectsCacher.removeObject(gameObject);
        Destroy(gameObject);
    }
}