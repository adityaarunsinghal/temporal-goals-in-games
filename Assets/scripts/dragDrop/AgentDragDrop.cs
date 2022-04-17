using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDragDrop : MonoBehaviour
{
    void Start()
    {
        if (!AgentStatus.active)
        {
            GetComponent<AgentDragDrop>().enabled = false;
        }
    }
    public void setPosition(Vector3 mousePosition)
    {
        GetComponent<Rigidbody2D>().position = mousePosition;
    }
}
