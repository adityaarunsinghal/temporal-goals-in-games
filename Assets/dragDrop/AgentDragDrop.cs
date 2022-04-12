using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDragDrop : MonoBehaviour
{
    public void setPosition(Vector3 mousePosition)
    {
        GetComponent<Rigidbody2D>().position = mousePosition;
    }
}
