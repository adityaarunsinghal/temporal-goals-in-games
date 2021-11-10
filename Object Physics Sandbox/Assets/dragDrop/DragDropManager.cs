using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    private DragDrop4 dragDrop4;
    private DragDrop dragDrop1;
    public void onDragDrop1()
    {
        gameObject.AddComponent<DragDrop>();
    }

    public void onDragDrop4()
    {
        dragDrop4 = GetComponent<DragDrop4>();
        dragDrop4.enabled = true;
    }

    public void offDragDrop4()
    {
        dragDrop4 = GetComponent<DragDrop4>();
        dragDrop4.enabled = false;
    }

    public void offDragDrop1()
    {
        Destroy(GetComponent<DragDrop>());
    }
}