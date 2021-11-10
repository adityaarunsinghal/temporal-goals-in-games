using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customProperties : MonoBehaviour
{

    public bool isFrozen = false;

    void OnBecameInvisible()
    {
        Destroy(gameObject);
        Debug.Log("Off screen object destroyed!");
    }

}